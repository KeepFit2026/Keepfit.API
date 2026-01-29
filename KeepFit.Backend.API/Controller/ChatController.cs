using System.Security.Claims;
using KeepFit.Backend.API.Models.Routes;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.DTOs;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeepFit.Backend.API.Controller;

[ApiController]
[Route("api/v1/chats")]
[Authorize] 
public class ChatController(IChatService chatService) : ControllerBase
{
    private readonly IChatService _chatService = chatService;

    /// <summary>
    /// Démarre une conversation privée ou récupère l'existante
    /// </summary>
    [HttpPost(ApiRoutes.Chats.StartPrivateChatAsync)]
    public async Task<IActionResult> StartPrivateChat([FromBody] StartChatRequest request)
    {
        //Récupère l'id dans le token.
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("AccountId");
        Console.WriteLine(userIdClaim.Value);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int myId)) 
            return Unauthorized("Impossible de récupérer votre ID utilisateur.");

        try 
        {
            var conversationId = await _chatService.StartPrivateChatAsync(myId, request.TargetUserId);
            
            return Ok(new 
            { 
                conversationId = conversationId,
                message = "Conversation prête"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Une erreur est survenue lors de la création du chat.");
        }
    }

    /// <summary>
    /// Retourne les utilisateurs qui n'ont pas de chat privé.
    /// </summary>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Chats.GetUsersWithoutPrivateChatAsync)]
    public async Task<IActionResult> GetUsersWithoutPrivateChatAsync(
        [FromQuery] PaginationFilter filter,
        CancellationToken cancellationToken
        )
    {
        //Récupère l'id dans le token.
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("AccountId");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int myId)) 
            return Unauthorized("Impossible de récupérer votre ID utilisateur.");

        try 
        {
            var result = await _chatService.GetUsersWithoutPrivateChatAsync(filter, myId, cancellationToken);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Une erreur est survenue lors de la création du chat.");
        }
    }
}
