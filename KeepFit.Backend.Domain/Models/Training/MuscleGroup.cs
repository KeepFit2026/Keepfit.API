using KeepFit.Backend.Domain.contracts;

namespace KeepFit.Backend.Domain.Models.Training;

public class MuscleGroup: IEntity
{
    public Guid Id { get; set; }
    
    public String Name { get; set; }
    
    public List<Exercise> Exercises { get; set; } = new List<Exercise>();
}