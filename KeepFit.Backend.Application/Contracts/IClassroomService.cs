using KeepFit.Backend.Application.DTOs.Classroom;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;

namespace KeepFit.Backend.Application.Contracts;

public interface IClassroomService : IContract<ClassroomResponse, ClassroomDto>
{
    /// <summary>
    /// Récupère tous les exercices
    /// </summary>
    /// <returns></returns>
    Task<PageApiResponse<List<ClassroomResponse>>>  GetAllAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère un exercice.
    /// </summary>
    /// <param name="id">Id de l'exercice</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<PageApiResponse<ClassroomResponse>> GetAsync(
        PaginationFilter filter, 
        Guid id, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Créer un exercice
    /// </summary>
    /// <param name="dto">un exercice</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<ClassroomResponse> CreateAsync(ClassroomDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Suprimme un exercice
    /// </summary>
    /// <param name="id">Id de l'exercice</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}