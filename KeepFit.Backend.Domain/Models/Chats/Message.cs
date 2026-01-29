using KeepFit.Backend.Domain.contracts;

namespace KeepFit.Backend.Domain.Models.Chats;

public class Message : IEntity
{
    public Guid Id { get; set; }
    
    public string Content { get; set; }
    
    /// <summary>
    /// Date d'envoi
    /// </summary>
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Id de la conversation liée
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// Id de l'utilisateur qui a envoyé le message
    /// </summary>
    public Guid SenderId { get; set; }
    
    //Navigation
    public Conversation Conversation { get; set; }
    public User.User Sender { get; set; }
}