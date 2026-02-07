namespace KeepFit.Backend.Domain.Models.Training;

public class Seance
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public Guid ProgramId { get; set; }
    
    public int Duration { get; set; }
    
    public int DayIndex { get; set; }

    public bool? IsActive { get; set; } = true;
    
    public FitnessProgram Program { get; set; }
    
    public List<SeanceExercise> SeanceExercise { get; set; } = new List<SeanceExercise>();
}