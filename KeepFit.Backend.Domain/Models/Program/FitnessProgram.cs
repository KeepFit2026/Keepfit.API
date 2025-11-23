namespace KeepFit.Backend.Domain.Models.Program
{
    public class FitnessProgram
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
        
        /// <summary>
        /// Navigation vers la table de jointure
        /// </summary>
        public List<ProgramExercise> ProgramExercises { get; set; } = new List<ProgramExercise>();
    }
}
