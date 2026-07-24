using Maze3.PlayTestOps.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maze3.PlayTestOps.Api.Data;

public class PlayTestOpsDbContext : DbContext
{
    public PlayTestOpsDbContext(
        DbContextOptions<PlayTestOpsDbContext> options)
        : base(options)
    {
    }

    public DbSet<GameBuild> GameBuilds { get; set; }

    public DbSet<PlaytestSession> PlaytestSessions { get; set; }

    public DbSet<BugReport> BugReports { get; set; }

    public DbSet<FeedbackNote> FeedbackNotes { get; set; }
}