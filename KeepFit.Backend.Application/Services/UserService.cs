using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Application.DTOs.Users;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.User;
using KeepFit.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeepFit.Backend.Application.Services;

public class UserService(
    IGenericService<User> genericService,
    IMapper mapper,
    AppDbContext context
    ) : BaseService<User, UserResponse, UserDto>(genericService, mapper), IUserService
{
    private readonly IGenericService<User> _genericService = genericService;
    private readonly IMapper _mapper = mapper;
    private readonly AppDbContext _context = context;

    public override async Task<PageApiResponse<List<UserResponse>>> GetAllAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        // On doit réimplémenter ici car on a besoin du 'includes: u => u.Role'
        var users = await _genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            includes: u => u.Role, 
            cancellationToken: cancellationToken);
        
        if(users.TotalRecord == 0) 
            throw new NotFoundException("Aucun utilisateur trouvé");
        
        var response = _mapper.Map<List<UserResponse>>(users.Data);
        
        return new PageApiResponse<List<UserResponse>>(
            response,
            filter.PageNumber,
            filter.PageSize,
            users.TotalRecord
            );
    }

    public override async Task<PageApiResponse<UserResponse>> GetAsync(
        PaginationFilter filter,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: u => u.Id == id,
            includes: u => u.Role,
            cancellationToken: cancellationToken);

        if (result.TotalRecord == 0) 
            throw new NotFoundException("Aucun utilisateur trouvé");
        
        var entity = result.Data.First();
        var response = _mapper.Map<UserResponse>(entity);

        return new PageApiResponse<UserResponse>(
            response,
            filter.PageNumber,
            filter.PageSize,
            result.TotalRecord
        );
    }
    
    public async Task<PageApiResponse<List<UserResponse>>> GetUsersWithoutPrivateChatAsync(
        PaginationFilter filter,
        int myAccountId,
        CancellationToken cancellationToken = default)
    {
        var safeFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var me = await _context.User
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.AccountId == myAccountId, cancellationToken);

        if (me == null) 
        {
            return new PageApiResponse<List<UserResponse>>([], safeFilter.PageNumber, safeFilter.PageSize, 0);
        }

        Guid myGuid = me.Id;

        // Sous-requête : IDs des utilisateurs avec qui j'ai une conversation privée
        var usersWithPrivateChat = _context.ConversationParticipant
            .Where(cp => cp.UserId == myGuid && !cp.Conversation.IsGroup)
            .SelectMany(cp => cp.Conversation.Participants)
            .Where(p => p.UserId != myGuid)
            .Select(p => p.UserId);

        // Requête principale : utilisateurs SANS conversation privée avec moi
        var query = _context.User
            .AsNoTracking()
            .Where(u => u.Id != myGuid)
            .Where(u => !usersWithPrivateChat.Contains(u.Id));

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
}