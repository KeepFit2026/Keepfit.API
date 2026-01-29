using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs;
using KeepFit.Backend.Application.DTOs.Requests; 
using KeepFit.Backend.Application.DTOs.Responses; 
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Chats;
using KeepFit.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeepFit.Backend.Application.Services;

public class ChatService(
    IGenericService<Conversation> genericService,
    IUnitOfWork unitOfWork,
    AppDbContext context,
    IMapper mapper
    ) : IChatService
{
    private readonly IGenericService<Conversation> _genericService = genericService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> StartPrivateChatAsync(int myAccountId, int otherAccountId, CancellationToken cancellationToken = default)
    {
        // 1. Conversion AccountId (int) -> User ID (Guid)
        var me = await _context.User.FirstOrDefaultAsync(u => u.AccountId == myAccountId, cancellationToken: cancellationToken);
        var other = await _context.User.FirstOrDefaultAsync(u => u.AccountId == otherAccountId, cancellationToken: cancellationToken);

        if (me == null || other == null)
        {
            throw new NotFoundException("Utilisateur introuvable");
        }

        Guid myGuid = me.Id;
        Guid otherGuid = other.Id;

        // 2. Vérification existence conversation
        var result = await _genericService.GetAllAsync(
            pageNumber: 1,
            pageSize: 1,
            predicate: c => !c.IsGroup 
                            && c.Participants.Any(p => p.UserId == myGuid) 
                            && c.Participants.Any(p => p.UserId == otherGuid), 
            cancellationToken: cancellationToken);

        var existingConversation = result.Data.FirstOrDefault();

        if (existingConversation != null)
        {
            return existingConversation.Id;
        }
        
        //Création Transactionnelle
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try 
        {
            var newConversation = new Conversation
            {
                Id = Guid.NewGuid(),
                IsGroup = false,
                Name = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            await _genericService.CreateAsync(newConversation, cancellationToken);

            var participant1 = new ConversationParticipant
            {
                Id = Guid.NewGuid(),
                ConversationId = newConversation.Id,
                UserId = myGuid,
                JoinedAt = DateTime.UtcNow,
                LastReadAt = DateTime.UtcNow
            };

            var participant2 = new ConversationParticipant
            {
                Id = Guid.NewGuid(),
                ConversationId = newConversation.Id,
                UserId = otherGuid,
                JoinedAt = DateTime.UtcNow
            };

            await _genericService.LinkEntitiesAsync(participant1, cancellationToken);
            await _genericService.LinkEntitiesAsync(participant2, cancellationToken);
            
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return newConversation.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw; 
        }
    }

    public async Task<PageApiResponse<List<UserResponse>>> GetUsersWithoutPrivateChatAsync(
        PaginationFilter filter, 
        int myAccountId, 
        CancellationToken cancellationToken = default)
    {
        var safeFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var me = await _context.User.FirstOrDefaultAsync(u => u.AccountId == myAccountId, cancellationToken);
        
        if (me == null) 
        {
            return new PageApiResponse<List<UserResponse>>([], safeFilter.PageNumber, safeFilter.PageSize, 0);
        }

        var myGuid = me.Id;

        var excludedGuids = await _context.ConversationParticipant
            .Where(p => p.UserId == myGuid && !p.Conversation.IsGroup)
            .SelectMany(p => p.Conversation.Participants)
            .Where(other => other.UserId != myGuid)
            .Select(other => other.UserId)
            .ToListAsync(cancellationToken);

        var query = _context.User
            .Where(u => u.Id != myGuid && !excludedGuids.Contains(u.Id));

        var totalRecords = await query.CountAsync(cancellationToken);

        var pagedUsers = await query
            .OrderBy(u => u.Name)
            .Skip((safeFilter.PageNumber - 1) * safeFilter.PageSize)
            .Take(safeFilter.PageSize)
            .ToListAsync(cancellationToken);

        var responseData = _mapper.Map<List<UserResponse>>(pagedUsers);

        return new PageApiResponse<List<UserResponse>>(
            responseData,
            safeFilter.PageNumber,
            safeFilter.PageSize,
            totalRecords
        );
    }
    
    public async Task<PageApiResponse<List<ConversationResponse>>> GetMyPrivateConversationsAsync(
    PaginationFilter filter,
    int myAccountId,
    CancellationToken cancellationToken = default)
{
    var safeFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

    //Récupérer le GUID à partir de l'int
    var me = await _context.User
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.AccountId == myAccountId, cancellationToken);

    if (me == null) 
    {
        return new PageApiResponse<List<ConversationResponse>>([], safeFilter.PageNumber, safeFilter.PageSize, 0);
    }

    Guid myGuid = me.Id;
    
    // On cherche les conversations qui NE SONT PAS des groupes
    // ET où JE suis un des participants
    var query = _context.Conversation
        .AsNoTracking()
        .Include(c => c.Participants)
            .ThenInclude(p => p.User) // Important : On charge les Users pour avoir le Nom
        .Where(c => !c.IsGroup && c.Participants.Any(p => p.UserId == myGuid));
    
    //pagination
    var totalRecords = await query.CountAsync(cancellationToken);

    var conversations = await query
        .OrderByDescending(c => c.UpdatedAt) // Les plus récentes en haut
        .Skip((safeFilter.PageNumber - 1) * safeFilter.PageSize)
        .Take(safeFilter.PageSize)
        .ToListAsync(cancellationToken);
    
    var responseList = new List<ConversationResponse>();

    foreach (var conv in conversations)
    {
        // On cherche le participant qui n'est pas MOI
        var otherParticipant = conv.Participants.FirstOrDefault(p => p.UserId != myGuid);
        
        string displayName = otherParticipant?.User?.Name ?? "Utilisateur Inconnu";

        responseList.Add(new ConversationResponse
        {
            Id = conv.Id,
            Name = displayName,
            UpdatedAt = conv.UpdatedAt
        });
    }

    return new PageApiResponse<List<ConversationResponse>>(
        responseList,
        safeFilter.PageNumber,
        safeFilter.PageSize,
        totalRecords
    );
}
}