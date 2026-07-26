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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PlaytestSession>()
            .HasOne(session => session.GameBuild)
            .WithMany(build => build.PlaytestSessions)
            .HasForeignKey(session => session.GameBuildId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BugReport>()
            .HasOne(bug => bug.PlaytestSession)
            .WithMany(session => session.BugReports)
            .HasForeignKey(bug => bug.PlaytestSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FeedbackNote>()
            .HasOne(feedback => feedback.PlaytestSession)
            .WithMany(session => session.FeedbackNotes)
            .HasForeignKey(feedback => feedback.PlaytestSessionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}