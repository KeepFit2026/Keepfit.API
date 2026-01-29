using System.Security.Claims;
using KeepFit.Backend.API.Filter;
using KeepFit.Backend.API.Models.Routes;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Application.DTOs.Users;
using KeepFit.Backend.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
[Route("api/v1/users")]
[AuthorizeRole(UserRoles.Admin)]
public class UserController(
    IChatService chatService,
    IUserService service): 
    BaseGenericController<IUserService, UserResponse, UserDto>(service)
{
    private readonly IUserService _service = service;
    private readonly IChatService _chatService = chatService;
    
    [NonAction]
    public override Task<IActionResult> CreateAsync(
        [FromBody] UserDto dto, 
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IActionResult>(NotFound());
    }
    
    /// <summary>
    /// Récupère tous les utilisateurs disponible pour la création
    /// d'une conversation privé en fonction de l'utilisateur connecté.
    /// </summary>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Users.GetAvailableUsers)]
    public async Task<IActionResult> GetAvailableUsers(
        [FromQuery] PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        // Récupération de l'ID via le token JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("AccountId");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int myId))
            return Unauthorized();

        var users = await _service.GetUsersWithoutPrivateChatAsync(filter, myId, cancellationToken);

        return Ok(users);
    }
    
    /// <summary>
    /// Récupère les conversations privées de l'utilisateur connecté.
    /// </summary>
    [HttpGet(ApiRoutes.Users.MyConversations)] 
    public async Task<IActionResult> GetMyConversations(
        [FromQuery] PaginationFilter filter, 
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("AccountId");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int myId))
            return Unauthorized();

        var result = await _chatService.GetMyPrivateConversationsAsync(filter, myId, cancellationToken);

        return Ok(result);
    }
}