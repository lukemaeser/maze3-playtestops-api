namespace Maze3.PlayTestOps.Api.Models;

public class FeedbackNote
{
    public int Id { get; set; }

    public int PlaytestSessionId { get; set; }

    public string Category { get; set; } = string.Empty;

    public string Comment { get; set; } = string.Empty;

    public int Rating { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}