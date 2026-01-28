using KeepFit.Backend.Domain.contracts;
using KeepFit.Backend.Domain.Models.Program;

namespace KeepFit.Backend.Domain.Models;

public class Classroom : IEntity
{
    /// <summary>
    /// Id de la classe
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// nom de la classe
    /// </summary>
    public string Name { get; set; }
    
    public ICollection<ClassroomUser> ClassroomUsers { get; set; } = new List<ClassroomUser>();
}