namespace KeepFit.Backend.Application.DTOs.Responses;

public class ExerciseResponse
{
    /// <summary>
    /// Id de l'exercice
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Nom de l'exercice
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Description de l'exercice.
    /// </summary>
    public string Description { get; set; }
}