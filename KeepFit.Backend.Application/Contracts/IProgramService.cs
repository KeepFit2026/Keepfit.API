using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeepFit.Backend.Application.DTOs.Programs;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;

namespace KeepFit.Backend.Application.Contracts
{
    public interface IProgramService : IContract<ProgramResponse, ProgramDto>
    {
        Task<PageApiResponse<List<ProgramResponse>>> GetAllAsync(PaginationFilter filter,CancellationToken cancellationToken = default);
        Task<PageApiResponse<ProgramResponse?>> GetAsync(PaginationFilter filter, Guid id,
            CancellationToken cancellationToken = default);
        Task<ProgramResponse> CreateAsync(ProgramDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Récupère tous les exercices d'un programme.
        /// </summary>
        /// <param name="programId">L'id du programme</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        Task<List<ExerciseResponse>> GetAllExercisesFromProgramAsync(Guid programId, 
            CancellationToken cancellationToken = default);
    }
}
