namespace KeepFit.Backend.Domain.Models.Exercise;

public class Exercise
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
    /// Description de l'exercice
    /// </summary>
    public string Description { get; set; }
}