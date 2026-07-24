using Maze3.PlayTestOps.Api.Data;
using Maze3.PlayTestOps.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PlayTestOpsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PlayTestOpsDatabase")));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// GAME BUILDS

// GET all game builds
app.MapGet("/api/gamebuilds", async (PlayTestOpsDbContext db) =>
{
    var gameBuilds = await db.GameBuilds.ToListAsync();
    return Results.Ok(gameBuilds);
});

// GET one game build by ID
app.MapGet("/api/gamebuilds/{id:int}", async (
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

// POST a new game build
app.MapPost("/api/gamebuilds", async (
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

    // SQL Server now generates the ID.
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

    return Results.Created(
        $"/api/gamebuilds/{newBuild.Id}",
        newBuild);
});

// PUT an existing game build
app.MapPut("/api/gamebuilds/{id:int}", async (
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

    return Results.Ok(build);
});

// DELETE a game build
app.MapDelete("/api/gamebuilds/{id:int}", async (
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

    return Results.NoContent();
});

// PLAYTEST SESSIONS

// GET all playtest sessions
app.MapGet("/api/sessions", async (PlayTestOpsDbContext db) =>
{
    var sessions = await db.PlaytestSessions.ToListAsync();
    return Results.Ok(sessions);
});

// GET one playtest session by ID
app.MapGet("/api/sessions/{id:int}", async (
    int id,
    PlayTestOpsDbContext db) =>
{
    var session = await db.PlaytestSessions.FindAsync(id);

    if (session is null)
    {
        return Results.NotFound("PlaytestSession not found.");
    }

    return Results.Ok(session);
});

// POST a new playtest session
app.MapPost("/api/sessions", async (
    PlaytestSession newSession,
    PlayTestOpsDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(newSession.TesterName))
    {
        return Results.BadRequest("TesterName is required.");
    }

    if (string.IsNullOrWhiteSpace(newSession.Platform))
    {
        return Results.BadRequest("Platform is required.");
    }

    newSession.Id = 0;

    if (newSession.SessionDate == default)
    {
        newSession.SessionDate = DateTime.UtcNow;
    }

    db.PlaytestSessions.Add(newSession);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/api/sessions/{newSession.Id}",
        newSession);
});

// PUT an existing playtest session
app.MapPut("/api/sessions/{id:int}", async (
    int id,
    PlaytestSession updatedSession,
    PlayTestOpsDbContext db) =>
{
    var session = await db.PlaytestSessions.FindAsync(id);

    if (session is null)
    {
        return Results.NotFound("PlaytestSession not found.");
    }

    session.GameBuildId = updatedSession.GameBuildId;
    session.TesterName = updatedSession.TesterName;
    session.Platform = updatedSession.Platform;
    session.SessionDate = updatedSession.SessionDate;
    session.Notes = updatedSession.Notes;

    await db.SaveChangesAsync();

    return Results.Ok(session);
});

// DELETE a playtest session
app.MapDelete("/api/sessions/{id:int}", async (
    int id,
    PlayTestOpsDbContext db) =>
{
    var session = await db.PlaytestSessions.FindAsync(id);

    if (session is null)
    {
        return Results.NotFound("PlaytestSession not found.");
    }

    db.PlaytestSessions.Remove(session);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// BUG REPORTS

// GET all bug reports
app.MapGet("/api/bugs", async (PlayTestOpsDbContext db) =>
{
    var bugs = await db.BugReports.ToListAsync();
    return Results.Ok(bugs);
});

// GET one bug report by ID
app.MapGet("/api/bugs/{id:int}", async (
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
app.MapPost("/api/bugs", async (
    BugReport newBug,
    PlayTestOpsDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(newBug.Title))
    {
        return Results.BadRequest("Title is required.");
    }

    if (string.IsNullOrWhiteSpace(newBug.Description))
    {
        return Results.BadRequest("Description is required.");
    }

    newBug.Id = 0;

    if (newBug.CreatedAt == default)
    {
        newBug.CreatedAt = DateTime.UtcNow;
    }

    db.BugReports.Add(newBug);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/api/bugs/{newBug.Id}",
        newBug);
});

// PUT an existing bug report
app.MapPut("/api/bugs/{id:int}", async (
    int id,
    BugReport updatedBug,
    PlayTestOpsDbContext db) =>
{
    var bug = await db.BugReports.FindAsync(id);

    if (bug is null)
    {
        return Results.NotFound("BugReport not found.");
    }

    bug.PlaytestSessionId = updatedBug.PlaytestSessionId;
    bug.Title = updatedBug.Title;
    bug.Description = updatedBug.Description;
    bug.Severity = updatedBug.Severity;
    bug.Status = updatedBug.Status;
    bug.ReproSteps = updatedBug.ReproSteps;

    await db.SaveChangesAsync();

    return Results.Ok(bug);
});

// DELETE a bug report
app.MapDelete("/api/bugs/{id:int}", async (
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

    return Results.NoContent();
});

// FEEDBACK NOTES

// GET all feedback notes
app.MapGet("/api/feedback", async (PlayTestOpsDbContext db) =>
{
    var feedbackNotes = await db.FeedbackNotes.ToListAsync();
    return Results.Ok(feedbackNotes);
});

// GET one feedback note by ID
app.MapGet("/api/feedback/{id:int}", async (
    int id,
    PlayTestOpsDbContext db) =>
{
    var feedback = await db.FeedbackNotes.FindAsync(id);

    if (feedback is null)
    {
        return Results.NotFound("FeedbackNote not found.");
    }

    return Results.Ok(feedback);
});

// POST a new feedback note
app.MapPost("/api/feedback", async (
    FeedbackNote newFeedback,
    PlayTestOpsDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(newFeedback.Category))
    {
        return Results.BadRequest("Category is required.");
    }

    if (string.IsNullOrWhiteSpace(newFeedback.Comment))
    {
        return Results.BadRequest("Comment is required.");
    }

    newFeedback.Id = 0;

    if (newFeedback.CreatedAt == default)
    {
        newFeedback.CreatedAt = DateTime.UtcNow;
    }

    db.FeedbackNotes.Add(newFeedback);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/api/feedback/{newFeedback.Id}",
        newFeedback);
});

// PUT an existing feedback note
app.MapPut("/api/feedback/{id:int}", async (
    int id,
    FeedbackNote updatedFeedback,
    PlayTestOpsDbContext db) =>
{
    var feedback = await db.FeedbackNotes.FindAsync(id);

    if (feedback is null)
    {
        return Results.NotFound("FeedbackNote not found.");
    }

    feedback.PlaytestSessionId = updatedFeedback.PlaytestSessionId;
    feedback.Category = updatedFeedback.Category;
    feedback.Comment = updatedFeedback.Comment;
    feedback.Rating = updatedFeedback.Rating;

    await db.SaveChangesAsync();

    return Results.Ok(feedback);
});

// DELETE a feedback note
app.MapDelete("/api/feedback/{id:int}", async (
    int id,
    PlayTestOpsDbContext db) =>
{
    var feedback = await db.FeedbackNotes.FindAsync(id);

    if (feedback is null)
    {
        return Results.NotFound("FeedbackNote not found.");
    }

    db.FeedbackNotes.Remove(feedback);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();