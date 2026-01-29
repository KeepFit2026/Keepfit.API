using KeepFit.Backend.Domain.contracts;

namespace KeepFit.Backend.Domain.Models.Chats;

public class Conversation : IEntity
{
    /// <summary>
    /// Id de la conversation
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Le nom de la conversation
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Si c'est un groupe
    /// </summary>
    public bool IsGroup { get; set; }
    
    /// <summary>
    /// Date de création
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date du dernier message (pour le tri)
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigations
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
}