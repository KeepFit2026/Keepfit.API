namespace KeepFit.Backend.Domain.Models.User;

public class Role
{
    /// <summary>
    /// Id du role
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nom du role
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Liste des utilisateurs qui ont ce role.
    /// </summary>
    public List<User> Users { get; set; }

    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    public Role()
    {
        Users = new List<User>();
    }
}