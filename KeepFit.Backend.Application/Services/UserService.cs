using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Application.DTOs.Users;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.User;
using KeepFit.Backend.Infrastructure;

namespace KeepFit.Backend.Application.Services;

public class UserService(
    IGenericService<User> genericService,
    IMapper mapper,
    AppDbContext dbContext
    ) : BaseService<User, UserResponse, UserDto>(genericService, mapper), IUserService
{
    private readonly IGenericService<User> _genericService = genericService;
    private readonly IMapper _mapper = mapper;

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

}