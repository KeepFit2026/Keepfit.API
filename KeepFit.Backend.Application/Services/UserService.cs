using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Application.DTOs.Users;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models;
using KeepFit.Backend.Domain.Models.User;
using KeepFit.Backend.Infrastructure;

namespace KeepFit.Backend.Application.Services;

public class UserService(
    IGenericService<User> genericService,
    IMapper mapper,
    AppDbContext dbContext
    ): IUserService
{
    public async Task<PageApiResponse<List<UserResponse>>> GetAllAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        var users = await genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            includes: u => u.Role,
            cancellationToken: cancellationToken);
        
        if(users.TotalRecord == 0) 
            throw new NotFoundException("Aucun exercice trouve");
        
        var response = mapper.Map<List<UserResponse>>(users.Data);
        
        return new PageApiResponse<List<UserResponse>>(
            response,
            filter.PageNumber,
            filter.PageSize,
            users.TotalRecord
            );
    }

    public async Task<PageApiResponse<UserResponse>> GetAsync(
        PaginationFilter filter,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: ex => ex.Id == id,
            includes: u => u.Role,
            cancellationToken: cancellationToken);

        if (user.TotalRecord == 0) throw new NotFoundException("Aucun exercice trouve");
        
        var exercise = user.Data.First();
        
        var response = mapper.Map<UserResponse>(exercise);

        return new PageApiResponse<UserResponse>(
            response,
            filter.PageNumber,
            filter.PageSize,
            user.TotalRecord
        );
    }

    public async Task<UserResponse> CreateAsync(
        UserDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await genericService.CreateAsync(
            mapper.Map<User>(dto), cancellationToken);
            
        return mapper.Map<UserResponse>(entity);
    }

    public async Task<bool> DeleteAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var result = await genericService.DeleteAsync(id, cancellationToken);
        if(!result) throw new NotFoundException("Aucun exercice trouve");
        return result;
    }
}