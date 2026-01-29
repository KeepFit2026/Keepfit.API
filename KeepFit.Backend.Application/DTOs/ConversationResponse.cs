namespace KeepFit.Backend.Application.DTOs;

public class ConversationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}