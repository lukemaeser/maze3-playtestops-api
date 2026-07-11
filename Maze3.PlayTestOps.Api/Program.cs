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

app.Run();