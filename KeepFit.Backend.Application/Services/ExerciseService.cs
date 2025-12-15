using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeepFit.Backend.Application.Services;

public class ExerciseService(
    IGenericService<Exercise> genericService,
    IMapper mapper,
    AppDbContext context
    ) : IExerciseService
{
    public async Task<List<ExerciseResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var exercises = await genericService.GetAllAsync(
            predicate: null,
            cancellationToken);
        
        if(exercises.Count == 0) throw new NotFoundException("Aucun exercice trouve");
        
        var response = mapper.Map<List<ExerciseResponse>>(exercises);

        foreach (var exercise in response)
        {
            exercise.ProgramsLink = $"exercises/{exercise.Id}/programs";
        }

        return response;
    }

    public async Task<ExerciseResponse> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var exercises = await genericService.GetAllAsync(
            predicate: ex => ex.Id == id,
            cancellationToken: cancellationToken);

        if (exercises == null || exercises.Count == 0) throw new NotFoundException("Aucun exercice trouve");
        
        var exercise = exercises.First();
        
        var response = mapper.Map<ExerciseResponse>(exercise);
        response.ProgramsLink = $"exercises/{response.Id}/programs";

        return response;
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

    public async Task<List<ProgramResponse>> GetProgramsFromExercise(
        Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        var programs = await context.ProgramExercise
            .Where(pe => pe.ExerciseId == exerciseId)
            .Select(pe => pe.Program)
            .ToListAsync(cancellationToken);
        
        if(programs.Count == 0) throw new NotFoundException("Aucun exercice trouve");
        
        return mapper.Map<List<ProgramResponse>>(programs);
    }

    public async Task<List<ProgramResponse>> GetProgramsWitoutNotExercises(Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        var programs = await context.FitnessProgram
            .Where(p => p.ProgramExercises.All(pe => pe.ExerciseId != exerciseId))
            .ToListAsync(cancellationToken);
        
        if(programs.Count == 0) throw new NotFoundException("Aucun programme trouve");
        return mapper.Map<List<ProgramResponse>>(programs);
    }
}
