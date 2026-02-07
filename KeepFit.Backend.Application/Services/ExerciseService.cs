using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Domain.Models.Training;
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
    
}