using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace KeepFit.Backend.Application.Services;

public class UnitOfWork(
    AppDbContext context, 
    IDbContextTransaction transaction) : IUnitOfWork
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private IDbContextTransaction _transaction = transaction;

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return _transaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}
