using Maze3.PlayTestOps.Api.Data;
using Maze3.PlayTestOps.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maze3.PlayTestOps.Api.Endpoints;

public static class PlaytestSessionEndpoints
{
    public static void MapPlaytestSessionEndpoints(this WebApplication app)
    {
        var routes = app
            .MapGroup("/api/sessions")
            .WithTags("Playtest Sessions");

        // GET all playtest sessions
        routes.MapGet("", async (PlayTestOpsDbContext db) =>
        {
            var sessions = await db.PlaytestSessions.ToListAsync();

            return Results.Ok(sessions);
        });

        // GET one playtest session by ID
        routes.MapGet("/{id:int}", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var session = await db.PlaytestSessions.FindAsync(id);

            if (session is null)
            {
                return Results.NotFound(
                    "PlaytestSession not found.");
            }

            return Results.Ok(session);
        });

        // GET all bug reports for one playtest session
        routes.MapGet("/{id:int}/bugs", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var sessionExists = await db.PlaytestSessions
                .AnyAsync(session => session.Id == id);

            if (!sessionExists)
            {
                return Results.NotFound(
                    $"PlaytestSession with ID {id} was not found.");
            }

            var bugs = await db.BugReports
                .Where(bug => bug.PlaytestSessionId == id)
                .ToListAsync();

            return Results.Ok(bugs);
        });

        // POST a new playtest session
        routes.MapPost("", async (
            PlaytestSession newSession,
            PlayTestOpsDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(newSession.TesterName))
            {
                return Results.BadRequest(
                    "TesterName is required.");
            }

            if (string.IsNullOrWhiteSpace(newSession.Platform))
            {
                return Results.BadRequest(
                    "Platform is required.");
            }

            newSession.Id = 0;

            if (newSession.SessionDate == default)
            {
                newSession.SessionDate = DateTime.UtcNow;
            }

            db.PlaytestSessions.Add(newSession);
            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Created playtest session {PlaytestSessionId} for game build {GameBuildId}",
                newSession.Id,
                newSession.GameBuildId);

            return Results.Created(
                $"/api/sessions/{newSession.Id}",
                newSession);
        });

        // PUT an existing playtest session
        routes.MapPut("/{id:int}", async (
            int id,
            PlaytestSession updatedSession,
            PlayTestOpsDbContext db) =>
        {
            var session = await db.PlaytestSessions.FindAsync(id);

            if (session is null)
            {
                return Results.NotFound(
                    "PlaytestSession not found.");
            }

            session.GameBuildId = updatedSession.GameBuildId;
            session.TesterName = updatedSession.TesterName;
            session.Platform = updatedSession.Platform;
            session.SessionDate = updatedSession.SessionDate;
            session.Notes = updatedSession.Notes;

            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Updated playtest session {PlaytestSessionId}",
                session.Id);

            return Results.Ok(session);
        });

        // DELETE a playtest session
        routes.MapDelete("/{id:int}", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var session = await db.PlaytestSessions.FindAsync(id);

            if (session is null)
            {
                return Results.NotFound(
                    "PlaytestSession not found.");
            }

            db.PlaytestSessions.Remove(session);
            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Deleted playtest session {PlaytestSessionId}",
                session.Id);

            return Results.NoContent();
        });
    }
}