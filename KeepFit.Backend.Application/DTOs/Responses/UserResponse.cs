using KeepFit.Backend.Domain.Models.User;

namespace KeepFit.Backend.Application.DTOs.Responses;

public class UserResponse
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
    /// Role de l'utilisateur
    /// </summary>
    public String RoleName { get; set; }
}