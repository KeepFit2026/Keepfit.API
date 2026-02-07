using Microsoft.AspNetCore.Mvc;
using KeepFit.Backend.API.Models.Routes;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Exceptions;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Programs;
using KeepFit.Backend.Application.DTOs.Requests;

namespace KeepFit.Backend.API.Controller
{
    [ApiController]
    [Route("api/v1/programs")]
    public class ProgramController(IProgramService service) :
        BaseGenericController<IProgramService, ProgramResponse, ProgramDto>(service)
    {
        
    }
}
