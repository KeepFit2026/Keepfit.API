using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeepFit.Backend.Application.DTOs.Programs;
using KeepFit.Backend.Application.DTOs.Responses;

namespace KeepFit.Backend.Application.Contracts
{
    public interface IProgramService
    {
        Task<List<ProgramResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ProgramResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ProgramResponse> CreateProgramAsync(ProgramDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteProgramAsync(Guid id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Ajoute une exercice à un programme.
        /// </summary>
        /// <param name="programId">Id du programme</param>
        /// <param name="exerciseId">Id de l'exercice</param>
        /// <param name="cancellationToken">Cancellartion Token</param>
        /// <returns></returns>
        Task<bool> AddExerciseToProgramAsync(Guid programId, Guid exerciseId, CancellationToken cancellationToken = default);
    }
}
