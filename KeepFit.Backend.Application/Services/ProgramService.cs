using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Programs;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Domain.Exceptions;

public class ProgramService(
    IGenericService<FitnessProgram> genericService,
    IMapper mapper
) : IProgramService
{
    public async Task<List<ProgramResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var programs = await genericService.GetAllAsync(cancellationToken);
        if (programs.Count == 0) throw new NotFoundException("Aucun programme trouvé");
        return mapper.Map<List<ProgramResponse>>(programs);
    }

    public async Task<ProgramResponse> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var program = await genericService.GetByIdAsync(id, cancellationToken);
        if (program == null) throw new NotFoundException("Aucun programme trouvé");
        return mapper.Map<ProgramResponse>(program);
    }

    public async Task<ProgramResponse> CreateProgramAsync(ProgramDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await genericService.CreateAsync(
            mapper.Map<FitnessProgram>(dto),
            cancellationToken
        );

        return mapper.Map<ProgramResponse>(entity);
    }

    public async Task<bool> DeleteProgramAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await genericService.DeleteAsync(id, cancellationToken);
        if (!result) throw new NotFoundException("Aucun programme trouvé");
        return result;
    }
}