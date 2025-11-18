namespace KeepFit.Backend.Application.DTOs.Responses
{
    public class ProgramResponse
    {
        /// <summary>
        /// Id du programme
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nom du programme
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Description du programme
        /// </summary>
        public required string Description { get; set; }
    }
}
