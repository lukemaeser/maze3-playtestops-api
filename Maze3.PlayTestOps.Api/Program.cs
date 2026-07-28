using Maze3.PlayTestOps.Api.Data;
using Maze3.PlayTestOps.Api.Endpoints;
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

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "MAZE3 PlayTestOps API v1");
    });
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<PlayTestOpsDbContext>();

    await SeedData.InitializeAsync(db);
}

app.UseHttpsRedirection();

app.MapGameBuildEndpoints();
app.MapPlaytestSessionEndpoints();
app.MapBugReportEndpoints();
app.MapFeedbackNoteEndpoints();

app.Run();