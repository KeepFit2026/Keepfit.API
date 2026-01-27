using System.Linq.Expressions;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace KeepFit.Backend.Application.Services;

public class GenericService<T> : IGenericService<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    private readonly IMemoryCache _cache;

    public GenericService(AppDbContext context, IMemoryCache cache, ILogger<GenericService<T>> logger)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _cache = cache;
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

        string cacheKey = $"{typeof(T).Name}_{id}";
        _cache.Remove(cacheKey);

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

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        //Génère une 'clé'. 
        string cacheKey = $"{typeof(T).Name}_{id}";
        
        //Vérifie si la clé existe déjà dans le cache.
        if (_cache.TryGetValue(cacheKey, out bool resultatEnCache)) return resultatEnCache;
        
        var entity = await _context.Set<T>().FindAsync([id], cancellationToken);
        var existsInDb = entity != null;
        
        //Met en cache pendant 5 minutes.
        _cache.Set(cacheKey, existsInDb, TimeSpan.FromMinutes(5));
        return existsInDb;
    }
}