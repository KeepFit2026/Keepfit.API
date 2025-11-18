using AutoMapper;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Programs;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.Program;

namespace KeepFit.Backend.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Exercise, ExerciseResponse>();
        
        CreateMap<ExerciseDto, Exercise>();

        CreateMap<FitnessProgram, ProgramResponse>();
        CreateMap<ProgramDto, FitnessProgram>();
    }
}