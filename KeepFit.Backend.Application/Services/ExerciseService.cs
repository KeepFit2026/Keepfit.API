using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Infrastructure;

namespace KeepFit.Backend.Application.Services;

public class ExerciseService(
    IGenericService<Exercise> genericService,
    IGenericService<FitnessProgram> genericProgramService,
    IMapper mapper
    ) : BaseService<Exercise, ExerciseResponse, ExerciseDto>(genericService, mapper), IExerciseService
{
    private readonly IGenericService<Exercise> _genericService = genericService;
    private readonly IMapper _mapper = mapper;

    // OVERRIDE : On garde ta logique spécifique pour ajouter les liens
    public override async Task<PageApiResponse<List<ExerciseResponse>>> GetAllAsync(
        PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        var pageResponse = await base.GetAllAsync(filter, cancellationToken);

        //On ajoute ta logique de liens
        foreach (var exercise in pageResponse.Data)
        {
            exercise.ProgramsLink = $"exercises/{exercise.Id}/programs";
        }

        return pageResponse;
    }
    
    public override async Task<PageApiResponse<ExerciseResponse>> GetAsync(
        PaginationFilter filter,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var pageResponse = await base.GetAsync(filter, id, cancellationToken);

        pageResponse.Data.ProgramsLink = $"exercises/{pageResponse.Data.Id}/programs";

        return pageResponse;
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
            cancellationToken: cancellationToken
        );
        
        var response =  _mapper.Map<List<ProgramResponse>>(result.Data);
        
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
            cancellationToken: cancellationToken
        );
        
        var responseData = _mapper.Map<List<ProgramResponse>>(result.Data);
    
        return new PageApiResponse<List<ProgramResponse>>(
            responseData,
            filter.PageNumber,
            filter.PageSize,
            result.TotalRecord);
    }
    
    public async Task<bool> AddExerciseToProgramAsync(
        Guid programId, Guid exerciseId, CancellationToken cancellationToken = default)
    {
        var programExist = await genericProgramService.ExistsAsync(programId, cancellationToken);
        var exerciseExist = await _genericService.ExistsAsync(exerciseId, cancellationToken); // Ici on utilise le genericService injecté dans le parent
        
        if (!programExist || !exerciseExist)
            throw new NotFoundException("Le programme ou l'exercice n'existe pas.");
        
        var link = new ProgramExercise
        {
            ProgramId = programId,
            ExerciseId = exerciseId
        };
        
        return await _genericService.LinkEntitiesAsync(link, cancellationToken);
    }
}