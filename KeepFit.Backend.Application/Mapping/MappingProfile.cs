using AutoMapper;
using KeepFit.Backend.Application.DTOs.Classroom;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Programs;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Application.DTOs.Users;
using KeepFit.Backend.Domain;
using KeepFit.Backend.Domain.Models;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Domain.Models.User;

namespace KeepFit.Backend.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Exercise, ExerciseResponse>();
        CreateMap<ExerciseDto, Exercise>();

        CreateMap<FitnessProgram, ProgramResponse>();
        CreateMap<ProgramDto, FitnessProgram>();
        
        CreateMap<Classroom, ClassroomResponse>();
        CreateMap<ClassroomDto, Classroom>();
        
        CreateMap<User, UserResponse>()
            //Liaison avec RoleName et RoleId
            .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Role.Name));
        
        CreateMap<UserDto, User>();
    }
}