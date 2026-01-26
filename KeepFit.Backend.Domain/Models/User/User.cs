namespace KeepFit.Backend.Domain.Models.User;

public class User
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
}