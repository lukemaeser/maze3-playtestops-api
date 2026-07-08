using Maze3.PlayTestOps.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Temporary in-memory data store.
// Later, this will be replaced by EF Core + SQL.
var gameBuilds = new List<GameBuild>
{
    new GameBuild
    {
        Id = 1,
        ProjectName = "It Waits in the Deep",
        Version = "0.0.1",
        Branch = "main",
        BuildDate = DateTime.UtcNow,
        ReleaseNotes = "Initial prototype build for interaction testing."
    }
};

// READ all builds
app.MapGet("/api/gamebuilds", () =>
{
    return Results.Ok(gameBuilds);
});

// READ one build by id
app.MapGet("/api/gamebuilds/{id:int}", (int id) =>
{
    var build = gameBuilds.FirstOrDefault(build => build.Id == id);

    return build is not null
        ? Results.Ok(build)
        : Results.NotFound();
});

// CREATE a new build
app.MapPost("/api/gamebuilds", (GameBuild newBuild) =>
{
    var nextId = gameBuilds.Count == 0
        ? 1
        : gameBuilds.Max(build => build.Id) + 1;

    newBuild.Id = nextId;
    newBuild.BuildDate = newBuild.BuildDate == default
        ? DateTime.UtcNow
        : newBuild.BuildDate;

    gameBuilds.Add(newBuild);

    return Results.Created($"/api/gamebuilds/{newBuild.Id}", newBuild);
});

// UPDATE an existing build
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

// DELETE a build
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