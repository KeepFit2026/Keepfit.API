using System.Linq.Expressions;

namespace KeepFit.Backend.Application.Contracts;

public interface IGenericService<T> where T : class
{

    /// <summary>
    /// Récupère tous les élements selon ou non un predicat
    /// </summary>
    /// <param name="predicate">fonction de filtre optionnel</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns></returns>
    Task<List<T> > GetAllAsync(Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Créer un élement
    /// </summary>
    /// <param name="entity">une entité</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <typeparam name="T">un Type</typeparam>
    /// <returns></returns>
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Supprime un élement
    /// </summary>
    /// <param name="id">Id de l'élement</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <typeparam name="T">un type</typeparam>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}