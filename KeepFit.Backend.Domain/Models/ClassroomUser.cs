namespace KeepFit.Backend.Domain.Models;

public class ClassroomUser
{
    /// <summary>
    /// Id de l'étudiant
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Id de la classe.
    /// </summary>
    public Guid ClassroomId { get; set; }
    
    public Classroom Classroom { get; set; }
    
    public User.User User { get; set; }
}