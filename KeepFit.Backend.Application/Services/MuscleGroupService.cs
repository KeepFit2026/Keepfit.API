using AutoMapper;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Models.Training;

namespace KeepFit.Backend.Application.Services;

public class MuscleGroupService(
    IGenericService<MuscleGroup> service,
    IMapper mapper
    ) : BaseService<MuscleGroup, MuscleGroupResponse, MuscleGroupDto>(service, mapper), IMuscleGroup
{
    
}