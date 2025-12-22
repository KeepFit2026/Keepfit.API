using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Programs;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeepFit.Backend.Application.Services;

public class ProgramService(
    IGenericService<FitnessProgram> genericService,
    AppDbContext context,
    IMapper mapper
) : IProgramService
{
    
    public async Task<PageApiResponse<List<ProgramResponse>>>GetAllAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        var result = await genericService.GetAllAsync(
            pageNumber : filter.PageNumber,
            pageSize : filter.PageSize,
            predicate: null,
            asNoTracking: false,
            cancellationToken
            );
        if (result.TotalRecord == 0) 
            throw new NotFoundException("Aucun programme trouvé");
        
        var response =  mapper.Map<List<ProgramResponse>>(result.Data);
        
        return new PageApiResponse<List<ProgramResponse>>(
            response,
            filter.PageNumber,
            filter.PageSize,
            result.TotalRecord
            );
    }

    public async Task<PageApiResponse<ProgramResponse?>> GetAsync(PaginationFilter filter,
        Guid id, CancellationToken cancellationToken = default)
    {
        var result = await genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: pro => pro.Id == id,
            asNoTracking: false,
            cancellationToken);
        
        if (result.TotalRecord == 0) 
            throw new NotFoundException("Aucun programme trouvé");
        
        var response = mapper.Map<ProgramResponse>(result.Data.First());

        return new PageApiResponse<ProgramResponse?>(
            response,
            filter.PageNumber,
            filter.PageSize,
            result.TotalRecord
        );
    }

    public async Task<ProgramResponse> CreateAsync(ProgramDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await genericService.CreateAsync(
            mapper.Map<FitnessProgram>(dto),
            cancellationToken
        );

        return mapper.Map<ProgramResponse>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await genericService.DeleteAsync(id, cancellationToken);
        if (!result) throw new NotFoundException("Aucun programme trouvé");
        return result;
    }

    public async Task<List<ExerciseResponse>> GetAllExercisesFromProgramAsync(
        Guid programId, CancellationToken cancellationToken = default)
    {
        var programExercises = await context.ProgramExercise
            .Where(pe => pe.ProgramId == programId)
            .Include(pe => pe.Exercise)
            .OrderBy(pe => pe.ExerciseId)
            .ToListAsync(cancellationToken);

        var exercises = programExercises.Select(pe => pe.Exercise).ToList();
        var response = mapper.Map<List<ExerciseResponse>>(exercises);

        foreach (var exercise in response)
        {
            exercise.ProgramsLink = $"exercises/{exercise.Id}/programs";
        }
        
        return response;
    }
}