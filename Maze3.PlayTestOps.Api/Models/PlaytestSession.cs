using System.Text.Json.Serialization;

namespace Maze3.PlayTestOps.Api.Models;

public class PlaytestSession
{
    public int Id { get; set; }

    public int GameBuildId { get; set; }

    [JsonIgnore]
    public GameBuild? GameBuild { get; set; }

    public string TesterName { get; set; } = string.Empty;

    public string Platform { get; set; } = string.Empty;

    public DateTime SessionDate { get; set; }

    public string Notes { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<BugReport> BugReports { get; set; }
        = new List<BugReport>();

    [JsonIgnore]
    public ICollection<FeedbackNote> FeedbackNotes { get; set; }
        = new List<FeedbackNote>();
}