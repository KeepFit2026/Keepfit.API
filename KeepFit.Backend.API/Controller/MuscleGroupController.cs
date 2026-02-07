using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
[Route("api/v1/groups")]
public class MuscleGroupController(IMuscleGroup service) :
    BaseGenericController<IMuscleGroup, MuscleGroupResponse, MuscleGroupDto>(service)
{
    
}