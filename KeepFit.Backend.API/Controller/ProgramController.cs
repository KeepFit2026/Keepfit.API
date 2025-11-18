using Microsoft.AspNetCore.Mvc;
using KeepFit.Backend.API.Models.Routes;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Programs;

namespace KeepFit.Backend.API.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProgramController(IProgramService service) : ControllerBase
    {
        /// <summary>
        /// Récupère un programme
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Programs.GetAllPrograms)]
        public async Task<IActionResult> GetAllAsync(
      CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await service.GetAllAsync(cancellationToken);
                return Ok(new ApiResponse<List<ProgramResponse>>
                   (true, result, "Listes des programmes."));
            }
            catch (NotFoundException message)
            {
                return NotFound(new ApiResponse<List<ProgramResponse>>
                   (false, null, message.Message));
            }
        }

        /// <summary>
        /// Récupère un programme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Programs.GetProgram)]
        public async Task<IActionResult> GetAsync(
      [FromRoute] Guid id,
      CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await service.GetAsync(id, cancellationToken);
                return Ok(new ApiResponse<ProgramResponse>(
                   true, result, "Programme Recupere"));
            }
            catch (NotFoundException message)
            {
                return NotFound(new ApiResponse<ProgramResponse>
                   (false, null, message.Message));
            }

        }

        /// <summary>
        /// Créer un programme
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Programs.CreateProgram)]
        public async Task<IActionResult> CreateAsync(
      [FromBody] ProgramDto dto,
      CancellationToken cancellationToken = default)
        {
            var result = await service.CreateProgramAsync(dto, cancellationToken);
            return Ok(new ApiResponse<ProgramResponse>(
               true, result, "Programme cree"));
        }


        /// <summary>
        /// Suprimme un programme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpDelete(ApiRoutes.Programs.DeleteProgram)]
        public async Task<IActionResult> DeleteAsync(
      [FromRoute] Guid id,
      CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await service.DeleteProgramAsync(id, cancellationToken);
                return Ok(result);
            }
            catch (NotFoundException message)
            {
                return NotFound(new ApiResponse<ExerciseResponse>
                   (false, null, message.Message));
            }
        }
    }
}
