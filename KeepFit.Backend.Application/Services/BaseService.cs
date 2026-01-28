using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.contracts;
using KeepFit.Backend.Domain.Exceptions;
namespace KeepFit.Backend.Application.Services;

public abstract class BaseService<TEntity, TResponse, TRequest>(
    IGenericService<TEntity> genericService,
    IMapper mapper) 
    where TEntity : class, IEntity
{
    public virtual async Task<PageApiResponse<List<TResponse>>> GetAllAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        var result = await genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: null,
            cancellationToken: cancellationToken);

        if (result.TotalRecord == 0)
            throw new NotFoundException($"Aucun enregistrement trouvé pour {typeof(TEntity).Name}");

        var response = mapper.Map<List<TResponse>>(result.Data);

        return new PageApiResponse<List<TResponse>>(
            response,
            filter.PageNumber,
            filter.PageSize,
            result.TotalRecord
        );
    }

    public virtual async Task<PageApiResponse<TResponse>> GetAsync(
        PaginationFilter filter,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // On utilise ici le predicate grâce à l'interface IEntity
        var result = await genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: x => x.Id == id, 
            cancellationToken: cancellationToken);

        if (result.TotalRecord == 0)
            throw new NotFoundException($"Entité {typeof(TEntity).Name} non trouvée (ID: {id})");

        var entity = result.Data.First();
        var response = mapper.Map<TResponse>(entity);

        return new PageApiResponse<TResponse>(
            response,
            filter.PageNumber,
            filter.PageSize,
            result.TotalRecord
        );
    }

    public virtual async Task<TResponse> CreateAsync(
        TRequest dto, 
        CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<TEntity>(dto);
        var createdEntity = await genericService.CreateAsync(entity, cancellationToken);
        return mapper.Map<TResponse>(createdEntity);
    }

    public virtual async Task<bool> DeleteAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var result = await genericService.DeleteAsync(id, cancellationToken);
        if (!result) 
            throw new NotFoundException($"Impossible de supprimer : {typeof(TEntity).Name} introuvable");
        
        return result;
    }
}