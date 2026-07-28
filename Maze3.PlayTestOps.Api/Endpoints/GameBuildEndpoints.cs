using Maze3.PlayTestOps.Api.Data;
using Maze3.PlayTestOps.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maze3.PlayTestOps.Api.Endpoints;

public static class GameBuildEndpoints
{
    public static void MapGameBuildEndpoints(this WebApplication app)
    {
        var routes = app
            .MapGroup("/api/gamebuilds")
            .WithTags("Game Builds");

        // GET all game builds
        routes.MapGet("", async (PlayTestOpsDbContext db) =>
        {
            var gameBuilds = await db.GameBuilds.ToListAsync();

            return Results.Ok(gameBuilds);
        });

        // GET one game build by ID
        routes.MapGet("/{id:int}", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var build = await db.GameBuilds.FindAsync(id);

            if (build is null)
            {
                return Results.NotFound("GameBuild not found.");
            }

            return Results.Ok(build);
        });

        // GET all playtest sessions for one game build
        routes.MapGet("/{id:int}/sessions", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var buildExists = await db.GameBuilds
                .AnyAsync(build => build.Id == id);

            if (!buildExists)
            {
                return Results.NotFound(
                    $"GameBuild with ID {id} was not found.");
            }

            var sessions = await db.PlaytestSessions
                .Where(session => session.GameBuildId == id)
                .ToListAsync();

            return Results.Ok(sessions);
        });

        // POST a new game build
        routes.MapPost("", async (
            GameBuild newBuild,
            PlayTestOpsDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(newBuild.ProjectName))
            {
                return Results.BadRequest("ProjectName is required.");
            }

            if (string.IsNullOrWhiteSpace(newBuild.Version))
            {
                return Results.BadRequest("Version is required.");
            }

            // SQL Server generates the ID.
            newBuild.Id = 0;

            if (newBuild.BuildDate == default)
            {
                newBuild.BuildDate = DateTime.UtcNow;
            }

            if (newBuild.CreatedAt == default)
            {
                newBuild.CreatedAt = DateTime.UtcNow;
            }

            db.GameBuilds.Add(newBuild);
            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Created game build {GameBuildId} for project {ProjectName}",
                newBuild.Id,
                newBuild.ProjectName);

            return Results.Created(
                $"/api/gamebuilds/{newBuild.Id}",
                newBuild);
        });

        // PUT an existing game build
        routes.MapPut("/{id:int}", async (
            int id,
            GameBuild updatedBuild,
            PlayTestOpsDbContext db) =>
        {
            var build = await db.GameBuilds.FindAsync(id);

            if (build is null)
            {
                return Results.NotFound("GameBuild not found.");
            }

            build.ProjectName = updatedBuild.ProjectName;
            build.Version = updatedBuild.Version;
            build.Branch = updatedBuild.Branch;
            build.BuildDate = updatedBuild.BuildDate;
            build.ReleaseNotes = updatedBuild.ReleaseNotes;

            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Updated game build {GameBuildId} for project {ProjectName}",
                build.Id,
                build.ProjectName);

            return Results.Ok(build);
        });

        // DELETE a game build
        routes.MapDelete("/{id:int}", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var build = await db.GameBuilds.FindAsync(id);

            if (build is null)
            {
                return Results.NotFound("GameBuild not found.");
            }

            db.GameBuilds.Remove(build);
            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Deleted game build {GameBuildId} for project {ProjectName}",
                build.Id,
                build.ProjectName);

            return Results.NoContent();
        });
    }
}