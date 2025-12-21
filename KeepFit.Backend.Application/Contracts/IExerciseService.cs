using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Models.Program;

namespace KeepFit.Backend.Application.Contracts;

public interface IExerciseService
{
    /// <summary>
    /// Récupère tous les exercices
    /// </summary>
    /// <returns></returns>
    Task<PageApiResponse<List<ExerciseResponse>>>  GetAllAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère un exercice.
    /// </summary>
    /// <param name="id">Id de l'exercice</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<PageApiResponse<ExerciseResponse>> GetAsync(
        PaginationFilter filter, 
        Guid id, CancellationToken cancellationToken = default);

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
    
    /// <summary>
    /// Récupère tous les programmes associés à cette exercice.
    /// </summary>
    /// <param name="exerciseId">Id de l'exercice.</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    Task<PageApiResponse<List<ProgramResponse>>>  GetProgramsFromExercise(
        PaginationFilter filter,
        Guid exerciseId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Récupère tous les programmes qui n'appartiennent pas à un exercice.
    /// </summary>
    /// <param name="exerciseId">Id de l'exercice</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    Task<PageApiResponse<List<ProgramResponse>>> GetProgramsWitoutNotExercises(
        PaginationFilter filter, 
        Guid exerciseId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Ajoute une exercice à un programme.
    /// </summary>
    /// <param name="programId">Id du programme</param>
    /// <param name="exerciseId">Id de l'exercice</param>
    /// <param name="cancellationToken">Cancellartion Token</param>
    /// <returns></returns>
    Task<bool> AddExerciseToProgramAsync(
        PaginationFilter filter,
        Guid programId, Guid exerciseId, 
        CancellationToken cancellationToken = default);

}