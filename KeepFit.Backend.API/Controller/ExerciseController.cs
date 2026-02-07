using KeepFit.Backend.API.Filter;
using KeepFit.Backend.API.Models.Routes;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Exercises;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Enums;
using KeepFit.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
// [AuthorizeRole(UserRoles.Admin)]
[Route("api/v1/exercises")]
public class ExerciseController(IExerciseService service) : 
    BaseGenericController<IExerciseService, ExerciseResponse, ExerciseDto>(service)
{
   
}