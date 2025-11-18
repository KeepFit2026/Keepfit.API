using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Infrastructure;

namespace KeepFit.Backend.Application.Services;

public class ExerciseService(
    IGenericService<Exercise> genericService,
    IMapper mapper
    ) : IExerciseService
{
    public async Task<List<ExerciseResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var exercises = await genericService.GetAllAsync(cancellationToken);
        if(exercises.Count == 0) throw new NotFoundException("Aucun exercice trouve");
        return mapper.Map<List<ExerciseResponse>>(exercises);
    }

    public async Task<ExerciseResponse> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var exercise = await genericService.GetByIdAsync(id, cancellationToken);
        if(exercise == null) throw new NotFoundException("Aucun exercice trouve");
        return mapper.Map<ExerciseResponse>(exercise);
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
}