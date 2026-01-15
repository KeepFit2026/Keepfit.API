using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Application.DTOs.Users;
using KeepFit.Backend.Domain.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
[Route("api/v1/users")]
public class UserController(
    IUserService service): 
    BaseGenericController<IUserService, UserResponse, UserDto>(service)
{
    [NonAction]
    public override Task<IActionResult> CreateAsync(
        [FromBody] UserDto dto, 
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IActionResult>(NotFound());
    }
}