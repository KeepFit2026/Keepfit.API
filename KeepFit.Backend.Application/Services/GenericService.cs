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

    public async Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
            query = query.Where(predicate);
        
        return await query.ToListAsync(cancellationToken: cancellationToken);
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
}