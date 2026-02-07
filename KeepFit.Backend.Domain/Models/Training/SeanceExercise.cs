namespace KeepFit.Backend.Domain.Models.Training;

public class SeanceExercise
{
    public Guid Id { get; set; }
    
    public Guid SeanceId { get; set; }
    
    public Guid ExerciseId { get; set; }

    public int? Duration { get; set; }
        
    public int? Reps { get; set; }

    public int Break { get; set; }
    
    public int Order { get; set; }
    
    public Seance Seance { get; set; }
    
    public Exercise Exercise { get; set; }
}