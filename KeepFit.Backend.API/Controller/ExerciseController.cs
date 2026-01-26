using KeepFit.Backend.API.Filter;
using KeepFit.Backend.API.Models.Routes;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Enums;
using KeepFit.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
[AuthorizeRole(UserRoles.Admin)]
[Route("api/v1/exercises")]
public class ExerciseController(IExerciseService service) : 
    BaseGenericController<IExerciseService, ExerciseResponse, ExerciseDto>(service)
{
    /// <summary>
    /// Récupère la liste des programmes associés à un exercice spécifique.
    /// </summary>
    /// <param name="filter">Filtres de pagination.</param>
    /// <param name="id">Identifiant de l'exercice.</param>
    /// <param name="cancellationToken">Token d’annulation.</param>
    /// <returns>Une liste de programmes.</returns>
    /// <response code="200">Retourne la liste des programmes contenant cet exercice.</response>
    /// <response code="404">Si l'exercice ou aucun programme n'est trouvé.</response>
    [HttpGet(ApiRoutes.Exercises.GetProgramsFromExercise)]
    [ProducesResponseType(typeof(PageApiResponse<List<ProgramResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<List<ProgramResponse>>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProgramsFromExercise(
        [FromQuery] PaginationFilter filter,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.GetProgramsFromExercise(filter, id, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException message)
        {
            return NotFound(new ApiResponse<List<ProgramResponse>?>(true, null, message.Message));
        }
    }

    /// <summary>
    /// Récupère tous les programmes qui ne contiennent PAS l'exercice spécifié.
    /// </summary>
    /// <param name="filter">Filtres de pagination.</param>
    /// <param name="id">Identifiant de l'exercice à exclure.</param>
    /// <param name="cancellationToken">Token d’annulation.</param>
    /// <returns>Une liste de programmes disponibles pour ajout.</returns>
    /// <response code="200">Retourne la liste des programmes ne contenant pas l'exercice.</response>
    /// <response code="404">Si l'exercice n'est pas trouvé.</response>
    [HttpGet(ApiRoutes.Exercises.GetProgramsWitoutNotExercises)]
    [ProducesResponseType(typeof(PageApiResponse<List<ProgramResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<List<ProgramResponse>>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProgramsWitoutNotExercises(
        [FromQuery] PaginationFilter filter,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.GetProgramsWitoutNotExercises(filter, id, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException message)
        {
            return NotFound(new ApiResponse<List<ProgramResponse>?>(true, null, message.Message));
        }
    }
    
    /// <summary>
    /// Ajoute un exercice existant à un programme spécifique.
    /// </summary>
    /// <param name="filter">Filtres de pagination (si applicable).</param>
    /// <param name="programId">Identifiant du programme cible.</param>
    /// <param name="exerciseId">Identifiant de l'exercice à ajouter.</param>
    /// <param name="cancellationToken">Token d’annulation.</param>
    /// <returns>Une confirmation de l'ajout.</returns>
    /// <response code="200">L'exercice a été ajouté au programme avec succès.</response>
    /// <response code="404">Si le programme ou l'exercice n'existe pas.</response>
    [HttpGet(ApiRoutes.Exercises.AddExerciseToProgram)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ProgramExerciseResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddExerciseToProgramAsync(
        [FromQuery] PaginationFilter filter,
        [FromRoute] Guid exerciseId,
        [FromRoute] Guid programId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await service.AddExerciseToProgramAsync(filter, programId, exerciseId, cancellationToken);
            return Ok(new ApiResponse<bool>(true, true, "Exercice ajouté !"));
        }
        catch (NotFoundException message)
        {
            return NotFound(new ApiResponse<ProgramExerciseResponse?>(true, null, message.Message));
        }
    }
}