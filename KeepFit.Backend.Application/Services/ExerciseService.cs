using System.Linq.Expressions;
using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Infrastructure;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace KeepFit.Backend.Application.Services;

public class ExerciseService(
    IGenericService<Exercise> genericService,
    IGenericService<FitnessProgram> genericProgramService,
    IMapper mapper,
    AppDbContext context
    ) : IExerciseService
{
    public async Task<PageApiResponse<List<ExerciseResponse>>> GetAllAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        var exercises = await genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: null,
            asNoTracking: false,
            cancellationToken);
        
        if(exercises.TotalRecord == 0) 
            throw new NotFoundException("Aucun exercice trouve");
        
        var response = mapper.Map<List<ExerciseResponse>>(exercises.Data);

        foreach (var exercise in response)
        {
            exercise.ProgramsLink = $"exercises/{exercise.Id}/programs";
        }

        return new PageApiResponse<List<ExerciseResponse>>(
            response,
            filter.PageNumber,
            filter.PageSize,
            exercises.TotalRecord
            );
    }

    public async Task<PageApiResponse<ExerciseResponse>> GetAsync(
        PaginationFilter filter,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var exercises = await genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: ex => ex.Id == id,
            cancellationToken: cancellationToken);

        if (exercises.TotalRecord == 0) throw new NotFoundException("Aucun exercice trouve");
        
        var exercise = exercises.Data.First();
        
        var response = mapper.Map<ExerciseResponse>(exercise);
        response.ProgramsLink = $"exercises/{response.Id}/programs";

        return new PageApiResponse<ExerciseResponse>(
            response,
            filter.PageNumber,
            filter.PageSize,
            exercises.TotalRecord
        );
    }

    public async Task<ExerciseResponse> CreateExerciseAsync(
        ExerciseDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await genericService.CreateAsync(
            mapper.Map<Exercise>(dto), cancellationToken);
            
        return mapper.Map<ExerciseResponse>(entity);
    }

    public async Task<bool> DeleteExerciseAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var result = await genericService.DeleteAsync(id, cancellationToken);
        if(!result) throw new NotFoundException("Aucun exercice trouve");
        return result;
    }

    public async Task<PageApiResponse<List<ProgramResponse>>> GetProgramsFromExercise(
        PaginationFilter filter,
        Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        var safeFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        
        var result = await genericProgramService.GetAllAsync(
            pageNumber: safeFilter.PageNumber,
            pageSize: safeFilter.PageSize,
            predicate: p => p.ProgramExercises.Any(pe => pe.ExerciseId == exerciseId),
            asNoTracking: false,
            cancellationToken
        );
        
        var response =  mapper.Map<List<ProgramResponse>>(result.Data);
        
        return new PageApiResponse<List<ProgramResponse>>(
            response,
            safeFilter.PageNumber,
            safeFilter.PageSize,
            result.TotalRecord
        );
    }

    public async Task<PageApiResponse<List<ProgramResponse>>> GetProgramsWitoutNotExercises(
        PaginationFilter filter,
        Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        var result = await genericProgramService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: p => p.ProgramExercises.All(pe => pe.ExerciseId != exerciseId),
            asNoTracking: false,
            cancellationToken
        );
        
        var responseData = mapper.Map<List<ProgramResponse>>(result.Data);
    
        return new PageApiResponse<List<ProgramResponse>>(
            responseData,
            filter.PageNumber,
            filter.PageSize,
            result.TotalRecord);
    }
    
    //TODO Refaire cette méthode, elle ne fonctionne plus avec l'ajout de la paginagtion.
    public async Task<bool> AddExerciseToProgramAsync(
        PaginationFilter filter,
        Guid programId, Guid exerciseId, CancellationToken cancellationToken = default)
    {
        var program = await genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: prog => prog.Id == programId,
            asNoTracking: false,
            cancellationToken);
        
        var exercise = await genericService.GetAllAsync(
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            predicate: ex => ex.Id == exerciseId,
            asNoTracking: false,
            cancellationToken);
        
        if(program.TotalRecord == 0)
            throw new NotFoundException("Aucun élément trouvé.");
        
        if(exercise.TotalRecord == 0)
            throw new NotFoundException("Aucun élément trouvé.");

        var programExercise = new ProgramExercise
        {
            ProgramId = programId,
            ExerciseId = exerciseId,
        };
        
        await context.ProgramExercise.AddAsync(programExercise, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
