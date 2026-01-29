using KeepFit.Backend.Domain.contracts;
using KeepFit.Backend.Domain.Models.Chats;

namespace KeepFit.Backend.Domain.Models.Chats;

public class ConversationParticipant : IEntity
{
    /// <summary>
    /// Id de la participation
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id de la conversation
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// Id de l'utilisateur participant
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Date de la dernière lecture des messages
    /// </summary>
    public DateTime? LastReadAt { get; set; }

    /// <summary>
    /// Date d'arrivée dans la conversation
    /// </summary>
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Navigations
    public Conversation Conversation { get; set; }
    public User.User User { get; set; }
}