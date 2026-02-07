namespace KeepFit.Backend.Domain.Models.Training;

public class MuscleGroup
{
    public Guid Id { get; set; }
    
    public String Name { get; set; }
    
    public List<Exercise> Exercises { get; set; } = new List<Exercise>();
}