using KeepFit.Backend.Application.DTOs;
using KeepFit.Backend.Application.DTOs.Requests;
using KeepFit.Backend.Application.DTOs.Responses;
using KeepFit.Backend.Domain.Models.User;

namespace KeepFit.Backend.Application.Contracts;

public interface IChatService
{
    /// <summary>
    /// Créer un nouveau chat privé entre deux utilisateurs
    /// </summary>
    /// <param name="myId">Id du user connecté</param>
    /// <param name="otherId">Id d'un autre utilisateur</param>
    /// <returns></returns>
    Task<Guid> StartPrivateChatAsync(int myId, int otherId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Prends les utilisateurs qui n'ont pas de chat privé. 
    /// </summary>
    /// <param name="myAccountId"></param>
    /// <returns></returns>
    Task<PageApiResponse<List<UserResponse>>> GetUsersWithoutPrivateChatAsync(PaginationFilter filter, int myAccountId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Permet de récupéré tous mes conversations privées ouverte. 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="myAccountId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PageApiResponse<List<ConversationResponse>>> GetMyPrivateConversationsAsync(PaginationFilter filter, int myAccountId, 
        CancellationToken cancellationToken = default);
}