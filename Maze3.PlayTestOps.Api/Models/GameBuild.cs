namespace Maze3.PlayTestOps.Api.Models;

public class GameBuild
{
    public int Id { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;

    public DateTime BuildDate { get; set; }

    public string ReleaseNotes { get; set; } = string.Empty;
}