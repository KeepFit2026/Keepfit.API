namespace KeepFit.Backend.Application.DTOs.Responses;

public class ProgramExerciseResponse
{
    /// <summary>
    /// Id du programme
    /// </summary>
    public Guid ProgramId { get; set; }
    
    /// <summary>
    /// Id de l'exerice associé au programme
    /// </summary>
    public Guid ExerciseId { get; set; }
}