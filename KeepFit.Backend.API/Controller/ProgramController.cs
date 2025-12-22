using Microsoft.AspNetCore.Mvc;
using KeepFit.Backend.API.Models.Routes;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Programs;
using KeepFit.Backend.Application.DTOs.Requests;

namespace KeepFit.Backend.API.Controller
{

    [ApiController]
    [Route("api/v1/programs")]
    public class ProgramController(IProgramService service) :
        BaseGenericController<IProgramService, ProgramResponse, ProgramDto>(service)
    {
        /// <summary>
        /// Récupère les exercices d'un programme.
        /// </summary>
        /// <param name="programId">Id du programme</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Programs.GetExercisesFromProgram)]
        public async Task<IActionResult> GetExercisesFromProgramAsync(
            [FromRoute] Guid programId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await service.GetAllExercisesFromProgramAsync(programId, cancellationToken);
                return Ok(new ApiResponse<List<ExerciseResponse>>(true, result, "Liste des exercices"));
            }
            catch (NotFoundException message)
            {
                return NotFound(new ApiResponse<List<ExerciseResponse?>?>(true, null, message.Message));
            }
        }
    }
}
