
using KeepFit.Backend.Domain.contracts;

namespace KeepFit.Backend.Domain.Models.Training
{
    public class FitnessProgram : IEntity
    {
        /// <summary>
        /// Id du programme
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nom du programme.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description du programme
        /// </summary>
        public string Description { get; set; }

        public bool? IsActive { get; set; } = true;
        
        public List<Seance> Seances { get; set; } = new List<Seance>();
    }
}
