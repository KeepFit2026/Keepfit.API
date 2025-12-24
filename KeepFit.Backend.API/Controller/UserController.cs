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
    [HttpGet]
    public override async Task<IActionResult> GetAllAsync(
        [FromQuery] PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await service.GetAllAsync(filter, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(new PageApiResponse<List<UserResponse>?>(null, 1, 1, 0 ));
        }
    }
    
    [NonAction]
    public override Task<IActionResult> CreateAsync(
        [FromBody] UserDto dto, 
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IActionResult>(NotFound());
    }
}