using Maze3.PlayTestOps.Api.Data;
using Maze3.PlayTestOps.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maze3.PlayTestOps.Api.Endpoints;

public static class BugReportEndpoints
{
    public static void MapBugReportEndpoints(this WebApplication app)
    {
        var routes = app
            .MapGroup("/api/bugs")
            .WithTags("Bug Reports");

        // GET all bug reports
        routes.MapGet("", async (PlayTestOpsDbContext db) =>
        {
            var bugs = await db.BugReports.ToListAsync();

            return Results.Ok(bugs);
        });

        // GET all unresolved bug reports
        routes.MapGet("/unresolved", async (
            PlayTestOpsDbContext db) =>
        {
            var unresolvedBugs = await db.BugReports
                .Where(bug =>
                    bug.Status == "Open" ||
                    bug.Status == "InProgress")
                .ToListAsync();

            return Results.Ok(unresolvedBugs);
        });

        // GET one bug report by ID
        routes.MapGet("/{id:int}", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var bug = await db.BugReports.FindAsync(id);

            if (bug is null)
            {
                return Results.NotFound("BugReport not found.");
            }

            return Results.Ok(bug);
        });

        // POST a new bug report
        routes.MapPost("", async (
            BugReport newBug,
            PlayTestOpsDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(newBug.Title))
            {
                return Results.BadRequest("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(newBug.Description))
            {
                return Results.BadRequest(
                    "Description is required.");
            }

            newBug.Id = 0;

            if (newBug.CreatedAt == default)
            {
                newBug.CreatedAt = DateTime.UtcNow;
            }

            db.BugReports.Add(newBug);
            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Created bug report {BugReportId} with severity {Severity}",
                newBug.Id,
                newBug.Severity);

            return Results.Created(
                $"/api/bugs/{newBug.Id}",
                newBug);
        });

        // PUT an existing bug report
        routes.MapPut("/{id:int}", async (
            int id,
            BugReport updatedBug,
            PlayTestOpsDbContext db) =>
        {
            var bug = await db.BugReports.FindAsync(id);

            if (bug is null)
            {
                return Results.NotFound("BugReport not found.");
            }

            bug.PlaytestSessionId =
                updatedBug.PlaytestSessionId;
            bug.Title = updatedBug.Title;
            bug.Description = updatedBug.Description;
            bug.Severity = updatedBug.Severity;
            bug.Status = updatedBug.Status;
            bug.ReproSteps = updatedBug.ReproSteps;

            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Updated bug report {BugReportId} with status {Status}",
                bug.Id,
                bug.Status);

            return Results.Ok(bug);
        });

        // DELETE a bug report
        routes.MapDelete("/{id:int}", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var bug = await db.BugReports.FindAsync(id);

            if (bug is null)
            {
                return Results.NotFound("BugReport not found.");
            }

            db.BugReports.Remove(bug);
            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Deleted bug report {BugReportId}",
                bug.Id);

            return Results.NoContent();
        });
    }
}