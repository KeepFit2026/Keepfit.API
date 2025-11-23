namespace KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Domain.Models.Exercise;

public class ProgramExercise
{
    /// <summary>
    /// Id du programme
    /// </summary>
    public Guid ProgramId { get; set; }
    
    /// <summary>
    /// Id de l'exerice associé au programme
    /// </summary>
    public Guid ExerciseId { get; set; }
    
    /// <summary>
    /// Binavigabilité entre exercise et programme.
    /// </summary>
    public FitnessProgram Program { get; set; }
    public Exercise Exercise { get; set; }
}