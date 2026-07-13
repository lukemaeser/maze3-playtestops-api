using Maze3.PlayTestOps.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Temporary in-memory data.
// This is a fake database for now.
// Later, EF Core + SQL Server / Azure SQL will replace this.
var gameBuilds = new List<GameBuild>
{
    new GameBuild
    {
        Id = 1,
        ProjectName = "It Waits in the Deep",
        Version = "0.0.1",
        Branch = "main",
        BuildDate = DateTime.UtcNow,
        ReleaseNotes = "Initial prototype/interactions build."
    }
};

var playtestSessions = new List<PlaytestSession>
{
    new PlaytestSession
    {
        Id = 1,
        GameBuildId = 1,
        TesterName = "Internal Tester 01",
        Platform = "Windows",
        SessionDate = DateTime.UtcNow,
        Notes = "Tester completed the main interaction loop and noted initial feedback."
    }
};

var bugReports = new List<BugReport>
{
    new BugReport
    {
        Id = 1,
        PlaytestSessionId = 1,
        Title = "Door prompt remains visible after interaction",
        Description = "The interaction prompt stays on screen after the player opens the door.",
        Severity = "Medium",
        Status = "Open",
        ReproSteps = "Start the build, approach the door, open the door, then step backward.",
        CreatedAt = DateTime.UtcNow
    }
};

var feedbackNotes = new List<FeedbackNote>
{
    new FeedbackNote
    {
        Id = 1,
        PlaytestSessionId = 1,
        Category = "Gameplay",
        Comment = "Door interaction worked, but the tester wanted stronger visual feedback.",
        Rating = 4,
        CreatedAt = DateTime.UtcNow
    }
};

// GET = read all game builds
app.MapGet("/api/gamebuilds", () =>
{
    return Results.Ok(gameBuilds);
});

// GET = read one game build by ID
app.MapGet("/api/gamebuilds/{id:int}", (int id) =>
{
    var build = gameBuilds.FirstOrDefault(build => build.Id == id);

    if (build is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(build);
});

// POST = create a new game build
app.MapPost("/api/gamebuilds", (GameBuild newBuild) =>
{
    var nextId = gameBuilds.Count == 0
        ? 1
        : gameBuilds.Max(build => build.Id) + 1;

    newBuild.Id = nextId;

    if (newBuild.BuildDate == default)
    {
        newBuild.BuildDate = DateTime.UtcNow;
    }

    gameBuilds.Add(newBuild);

    return Results.Created($"/api/gamebuilds/{newBuild.Id}", newBuild);
});

// PUT = update an existing game build
app.MapPut("/api/gamebuilds/{id:int}", (int id, GameBuild updatedBuild) =>
{
    var build = gameBuilds.FirstOrDefault(build => build.Id == id);

    if (build is null)
    {
        return Results.NotFound();
    }

    build.ProjectName = updatedBuild.ProjectName;
    build.Version = updatedBuild.Version;
    build.Branch = updatedBuild.Branch;
    build.BuildDate = updatedBuild.BuildDate;
    build.ReleaseNotes = updatedBuild.ReleaseNotes;

    return Results.Ok(build);
});

// DELETE = remove a game build
app.MapDelete("/api/gamebuilds/{id:int}", (int id) =>
{
    var build = gameBuilds.FirstOrDefault(build => build.Id == id);

    if (build is null)
    {
        return Results.NotFound();
    }

    gameBuilds.Remove(build);

    return Results.NoContent();
});

// READ all playtest sessions
app.MapGet("/api/sessions", () =>
{
    return Results.Ok(playtestSessions);
});

// READ one playtest session by id
app.MapGet("/api/sessions/{id:int}", (int id) =>
{
    var session = playtestSessions.FirstOrDefault(session => session.Id == id);

    return session is not null
        ? Results.Ok(session)
        : Results.NotFound();
});

// CREATE a new playtest session
app.MapPost("/api/sessions", (PlaytestSession newSession) =>
{
    var nextId = playtestSessions.Count == 0
        ? 1
        : playtestSessions.Max(session => session.Id) + 1;

    newSession.Id = nextId;

    if (newSession.SessionDate == default)
    {
        newSession.SessionDate = DateTime.UtcNow;
    }

    playtestSessions.Add(newSession);

    return Results.Created($"/api/sessions/{newSession.Id}", newSession);
});

// UPDATE an existing playtest session
app.MapPut("/api/sessions/{id:int}", (int id, PlaytestSession updatedSession) =>
{
    var session = playtestSessions.FirstOrDefault(session => session.Id == id);

    if (session is null)
    {
        return Results.NotFound();
    }

    session.GameBuildId = updatedSession.GameBuildId;
    session.TesterName = updatedSession.TesterName;
    session.Platform = updatedSession.Platform;
    session.SessionDate = updatedSession.SessionDate;
    session.Notes = updatedSession.Notes;

    return Results.Ok(session);
});

// DELETE a playtest session
app.MapDelete("/api/sessions/{id:int}", (int id) =>
{
    var session = playtestSessions.FirstOrDefault(session => session.Id == id);

    if (session is null)
    {
        return Results.NotFound();
    }

    playtestSessions.Remove(session);

    return Results.NoContent();
});

// READ all bug reports
app.MapGet("/api/bugs", () =>
{
    return Results.Ok(bugReports);
});

// READ one bug report by id
app.MapGet("/api/bugs/{id:int}", (int id) =>
{
    var bug = bugReports.FirstOrDefault(bug => bug.Id == id);

    if (bug is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(bug);
});

// CREATE a new bug report
app.MapPost("/api/bugs", (BugReport newBug) =>
{
    var nextId = bugReports.Count == 0
        ? 1
        : bugReports.Max(bug => bug.Id) + 1;

    newBug.Id = nextId;

    if (newBug.CreatedAt == default)
    {
        newBug.CreatedAt = DateTime.UtcNow;
    }

    bugReports.Add(newBug);

    return Results.Created($"/api/bugs/{newBug.Id}", newBug);
});

// UPDATE an existing bug report
app.MapPut("/api/bugs/{id:int}", (int id, BugReport updatedBug) =>
{
    var bug = bugReports.FirstOrDefault(bug => bug.Id == id);

    if (bug is null)
    {
        return Results.NotFound();
    }

    bug.PlaytestSessionId = updatedBug.PlaytestSessionId;
    bug.Title = updatedBug.Title;
    bug.Description = updatedBug.Description;
    bug.Severity = updatedBug.Severity;
    bug.Status = updatedBug.Status;
    bug.ReproSteps = updatedBug.ReproSteps;

    return Results.Ok(bug);
});

// DELETE a bug report
app.MapDelete("/api/bugs/{id:int}", (int id) =>
{
    var bug = bugReports.FirstOrDefault(bug => bug.Id == id);

    if (bug is null)
    {
        return Results.NotFound();
    }

    bugReports.Remove(bug);

    return Results.NoContent();
});

// READ all feedback notes
app.MapGet("/api/feedback", () =>
{
    return Results.Ok(feedbackNotes);
});

// READ one feedback note by id
app.MapGet("/api/feedback/{id:int}", (int id) =>
{
    var feedback = feedbackNotes.FirstOrDefault(feedback => feedback.Id == id);

    if (feedback is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(feedback);
});

// CREATE a new feedback note
app.MapPost("/api/feedback", (FeedbackNote newFeedback) =>
{
    var nextId = feedbackNotes.Count == 0
        ? 1
        : feedbackNotes.Max(feedback => feedback.Id) + 1;

    newFeedback.Id = nextId;

    if (newFeedback.CreatedAt == default)
    {
        newFeedback.CreatedAt = DateTime.UtcNow;
    }

    feedbackNotes.Add(newFeedback);

    return Results.Created($"/api/feedback/{newFeedback.Id}", newFeedback);
});

// UPDATE an existing feedback note
app.MapPut("/api/feedback/{id:int}", (int id, FeedbackNote updatedFeedback) =>
{
    var feedback = feedbackNotes.FirstOrDefault(feedback => feedback.Id == id);

    if (feedback is null)
    {
        return Results.NotFound();
    }

    feedback.PlaytestSessionId = updatedFeedback.PlaytestSessionId;
    feedback.Category = updatedFeedback.Category;
    feedback.Comment = updatedFeedback.Comment;
    feedback.Rating = updatedFeedback.Rating;

    return Results.Ok(feedback);
});

// DELETE a feedback note
app.MapDelete("/api/feedback/{id:int}", (int id) =>
{
    var feedback = feedbackNotes.FirstOrDefault(feedback => feedback.Id == id);

    if (feedback is null)
    {
        return Results.NotFound();
    }

    feedbackNotes.Remove(feedback);

    return Results.NoContent();
});

app.Run();