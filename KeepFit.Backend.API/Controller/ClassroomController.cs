using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Classroom;
using KeepFit.Backend.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
[Route("api/v1/classrooms")]
public class ClassroomController(IClassroomService service) :
    BaseGenericController<IClassroomService, ClassroomResponse, ClassroomDto>(service)
{
    
}