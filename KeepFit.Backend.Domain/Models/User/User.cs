namespace KeepFit.Backend.Domain.Models.User;

public class User
{
    /// <summary>
    /// Id de l'utilisateur
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nom de l'utilisateur
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Id du token t'authentification
    /// </summary>
    public int AccountId { get; set; }

    /// <summary>
    /// Id du role de l'utilisateur
    /// </summary>
    public int RoleId { get; set; }
    
    /// <summary>
    /// Binavigabilité vers Role.
    /// </summary>
    public Role Role { get; set; }
}