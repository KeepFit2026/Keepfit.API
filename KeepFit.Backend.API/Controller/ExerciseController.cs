using KeepFit.Backend.API.Models.Routes;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Program;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController(IExerciseService service) : ControllerBase
{
    /// <summary>
    /// Récupère tous les exercices.
    /// </summary>
    /// <param name="cancellationToken">Token d’annulation.</param>
    /// <returns>
    /// - 200 OK avec la liste des exercices
    /// - 404 NotFound si aucun exercice.
    /// </returns>
    [HttpGet(ApiRoutes.Exercises.GetAllExercises)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            var result = await service.GetAllAsync(cancellationToken);
            return Ok(new ApiResponse<List<ExerciseResponse>>(true, result, "Liste des exercices."));
        }
        catch (NotFoundException message)
        {
            return NotFound(new ApiResponse<List<ExerciseResponse>?>(true, null, message.Message));
        }
    }

    /// <summary>
    /// Récupère un exercice par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'exercice.</param>
    /// <param name="cancellationToken">Token d’annulation.</param>
    /// <returns>
    /// - 200 OK avec l'exercice
    /// - 404 NotFound si non trouvé.
    /// </returns>
    [HttpGet(ApiRoutes.Exercises.GetExercises)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.GetAsync(id, cancellationToken);
            return Ok(new ApiResponse<ExerciseResponse>(true, result, "Exercice récupéré"));
        }
        catch (NotFoundException message)
        {
            return NotFound(new ApiResponse<ExerciseResponse?>(true, null, message.Message));
        }
    }

    /// <summary>
    /// Créer un nouvel exercice.
    /// </summary>
    /// <param name="dto">Données de l'exercice à créer.</param>
    /// <param name="cancellationToken">Token d’annulation.</param>
    /// <returns>
    /// </returns>
    [HttpPost(ApiRoutes.Exercises.CreateExercises)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] ExerciseDto dto, 
        CancellationToken cancellationToken = default)
    {
        var result = await service.CreateExerciseAsync(dto, cancellationToken);
        return Ok(new ApiResponse<ExerciseResponse>(true, result, "Exercice créé"));
    }

    /// <summary>
    /// Supprime un exercice par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'exercice.</param>
    /// <param name="cancellationToken">Token d’annulation.</param>
    /// <returns>
    /// - 204 No Content si suppression réussie
    /// - 404 NotFound si l'exercice n’existe pas.
    /// </returns>
    [HttpDelete(ApiRoutes.Exercises.DeleteExercises)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await service.DeleteExerciseAsync(id, cancellationToken);
            return NoContent();
        }
        catch (NotFoundException message)
        {
            return NotFound(new ApiResponse<bool>(true, false, "Erreur: " + message.Message));
        }
    }

    [HttpGet(ApiRoutes.Exercises.GetProgramsFromExercise)]
    public async Task<IActionResult> GetProgramsFromExercise(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.GetProgramsFromExercise(id, cancellationToken);
            return Ok(new ApiResponse<List<ProgramResponse>>(true, result, "OUI"));
        }
        catch (NotFoundException message)
        {
            return NotFound(new ApiResponse<List<ProgramResponse>?>(true, null, message.Message));
        }
    }
}