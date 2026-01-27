using System.Linq.Expressions;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeepFit.Backend.Application.Services;

public class GenericService<T> : IGenericService<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericService(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<(List<T> Data, int TotalRecord)> GetAllAsync(int pageNumber, int pageSize, 
        Expression<Func<T, bool>>? predicate = null, bool asNoTracking = false,
        CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet;
        
        if (asNoTracking)
            query = query.AsNoTracking();
        
        if (predicate != null)
            query = query.Where(predicate);
        
        if(includes != null)
            foreach(var include in includes)
                query = query.Include(include);
        
        int totalRecords = await query.CountAsync(cancellationToken);
        
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return (data, totalRecords);
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken);
        if (entity == null) return false;

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    
    public async Task<bool> LinkEntitiesAsync<TLink>(
        TLink linkEntity, 
        CancellationToken cancellationToken = default) 
        where TLink : class
    {
        await _context.Set<TLink>().AddAsync(linkEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().AnyAsync(predicate, cancellationToken);
    }
}