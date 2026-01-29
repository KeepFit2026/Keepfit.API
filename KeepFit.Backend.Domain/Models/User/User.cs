using KeepFit.Backend.Domain.contracts;
using KeepFit.Backend.Domain.Models.Chats;

namespace KeepFit.Backend.Domain.Models.User;

public class User : IEntity
{
    /// <summary>
    /// Id de l'utilisateur
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Nom de l'utilisateur
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Id du token t'authentification
    /// </summary>
    public int AccountId { get; set; }
    
    public int RoleId { get; set; }
    
    public Role Role { get; set; }
    
    public ICollection<ClassroomUser> ClassroomUsers { get; set; } = new List<ClassroomUser>();
    
    public ICollection<Message> MessagesSent { get; set; } = new List<Message>();
    public ICollection<ConversationParticipant> Conversations { get; set; } = new List<ConversationParticipant>();
}