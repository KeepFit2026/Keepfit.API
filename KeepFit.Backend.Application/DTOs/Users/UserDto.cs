namespace KeepFit.Backend.Application.DTOs.Users;

public class UserDto
{
    /// <summary>
    /// Nom de l'utilisateur
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Id du role de l'utilisateur
    /// </summary>
    public int RoleId { get; set; }
}