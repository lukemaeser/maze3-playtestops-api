using Maze3.PlayTestOps.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maze3.PlayTestOps.Api.Data;

public static class SeedData
{
    public static async Task InitializeAsync(PlayTestOpsDbContext db)
    {
        var sampleAlreadyExists = await db.GameBuilds.AnyAsync(build =>
            build.ProjectName == "It Waits in the Deep" &&
            build.Version == "0.1.0");

        if (sampleAlreadyExists)
        {
            return;
        }

        var build = new GameBuild
        {
            ProjectName = "It Waits in the Deep",
            Version = "0.1.0",
            Branch = "prototype/interactions",
            BuildDate = DateTime.UtcNow,
            ReleaseNotes =
                "Added door interaction, lantern pickup, and first-pass feedback prompts.",
            CreatedAt = DateTime.UtcNow
        };

        var session = new PlaytestSession
        {
            TesterName = "Internal Tester 01",
            Platform = "Windows",
            SessionDate = DateTime.UtcNow,
            Notes =
                "Tester completed the main interaction loop but missed the lantern prompt."
        };

        session.BugReports.Add(new BugReport
        {
            Title = "Door prompt remains visible after door opens",
            Description =
                "The interaction prompt remains visible after the door has opened.",
            Severity = "Medium",
            Status = "Open",
            ReproSteps =
                "Start build 0.1.0, collect the lantern, approach the door, open it, then step backward.",
            CreatedAt = DateTime.UtcNow
        });

        session.FeedbackNotes.Add(new FeedbackNote
        {
            Category = "Gameplay",
            Rating = 4,
            Comment =
                "The door interaction worked, but stronger visual feedback would improve clarity.",
            CreatedAt = DateTime.UtcNow
        });

        build.PlaytestSessions.Add(session);

        db.GameBuilds.Add(build);
        await db.SaveChangesAsync();
    }
}