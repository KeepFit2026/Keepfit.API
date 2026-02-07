using KeepFit.Backend.Domain.contracts;
using KeepFit.Backend.Domain.Enums;

namespace KeepFit.Backend.Domain.Models.Training;

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
    /// Difficulté de l'exerice.
    /// </summary>
    public Difficulty Difficulty {get; set;}
    
    /// <summary>
    /// Description de l'exercice
    /// </summary>
    public string Description { get; set; }
    
    public Guid MuscleGroupId { get; set; }
    
    public MuscleGroup MuscleGroup { get; set; }
}