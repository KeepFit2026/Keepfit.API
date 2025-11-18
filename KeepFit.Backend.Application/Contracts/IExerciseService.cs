using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Responses;

namespace KeepFit.Backend.Application.Contracts;

public interface IExerciseService
{
    /// <summary>
    /// Récupère tous les exercices
    /// </summary>
    /// <returns></returns>
    Task<List<ExerciseResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère un exercice.
    /// </summary>
    /// <param name="id">Id de l'exercice</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<ExerciseResponse> GetAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Créer un exercice
    /// </summary>
    /// <param name="dto">un exercice</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<ExerciseResponse> CreateExerciseAsync(ExerciseDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Suprimme un exercice
    /// </summary>
    /// <param name="id">Id de l'exercice</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<bool> DeleteExerciseAsync(Guid id, CancellationToken cancellationToken = default);

}