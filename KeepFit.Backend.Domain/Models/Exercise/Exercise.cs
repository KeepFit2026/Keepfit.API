using KeepFit.Backend.Domain.contracts;
using KeepFit.Backend.Domain.Models.Program;

namespace KeepFit.Backend.Domain.Models.Exercise;

public class Exercise : IEntity
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
    
    /// <summary>
    /// Navigation vers la table de jointure
    /// </summary>
    public List<ProgramExercise> ProgramExercises { get; set; } = new List<ProgramExercise>();
}