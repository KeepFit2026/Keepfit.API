using Microsoft.EntityFrameworkCore.Storage;

namespace KeepFit.Backend.Application.Contracts;

public interface IUnitOfWork: IDisposable
{   
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
    
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}