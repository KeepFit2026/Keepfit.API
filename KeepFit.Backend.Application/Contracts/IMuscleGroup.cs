using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;

namespace KeepFit.Backend.Application.Contracts;

public interface IMuscleGroup : IContract<MuscleGroupResponse, MuscleGroupDto>
{
    /// <summary>
    /// Récupère tous les Groupes
    /// </summary>
    /// <returns></returns>
    Task<PageApiResponse<List<MuscleGroupResponse>>>  GetAllAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère un Groupe.
    /// </summary>
    /// <param name="id">Id du groupe</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<PageApiResponse<MuscleGroupResponse>> GetAsync(
        PaginationFilter filter, 
        Guid id, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Créer un Groupe
    /// </summary>
    /// <param name="dto">un groupe</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<MuscleGroupResponse> CreateAsync(MuscleGroupDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Suprimme un exercice
    /// </summary>
    /// <param name="id">Id du group</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}