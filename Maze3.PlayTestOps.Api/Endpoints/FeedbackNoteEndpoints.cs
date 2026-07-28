using Maze3.PlayTestOps.Api.Data;
using Maze3.PlayTestOps.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maze3.PlayTestOps.Api.Endpoints;

public static class FeedbackNoteEndpoints
{
    public static void MapFeedbackNoteEndpoints(
        this WebApplication app)
    {
        var routes = app
            .MapGroup("/api/feedback")
            .WithTags("Feedback Notes");

        // GET all feedback notes
        routes.MapGet("", async (PlayTestOpsDbContext db) =>
        {
            var feedbackNotes =
                await db.FeedbackNotes.ToListAsync();

            return Results.Ok(feedbackNotes);
        });

        // GET feedback notes by category
        routes.MapGet("/category/{category}", async (
            string category,
            PlayTestOpsDbContext db) =>
        {
            var feedbackNotes = await db.FeedbackNotes
                .Where(feedback =>
                    feedback.Category == category)
                .ToListAsync();

            return Results.Ok(feedbackNotes);
        });

        // GET one feedback note by ID
        routes.MapGet("/{id:int}", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var feedback = await db.FeedbackNotes.FindAsync(id);

            if (feedback is null)
            {
                return Results.NotFound(
                    "FeedbackNote not found.");
            }

            return Results.Ok(feedback);
        });

        // POST a new feedback note
        routes.MapPost("", async (
            FeedbackNote newFeedback,
            PlayTestOpsDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(
                newFeedback.Category))
            {
                return Results.BadRequest(
                    "Category is required.");
            }

            if (string.IsNullOrWhiteSpace(
                newFeedback.Comment))
            {
                return Results.BadRequest(
                    "Comment is required.");
            }

            newFeedback.Id = 0;

            if (newFeedback.CreatedAt == default)
            {
                newFeedback.CreatedAt = DateTime.UtcNow;
            }

            db.FeedbackNotes.Add(newFeedback);
            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Created feedback note {FeedbackNoteId} in category {Category}",
                newFeedback.Id,
                newFeedback.Category);

            return Results.Created(
                $"/api/feedback/{newFeedback.Id}",
                newFeedback);
        });

        // PUT an existing feedback note
        routes.MapPut("/{id:int}", async (
            int id,
            FeedbackNote updatedFeedback,
            PlayTestOpsDbContext db) =>
        {
            var feedback = await db.FeedbackNotes.FindAsync(id);

            if (feedback is null)
            {
                return Results.NotFound(
                    "FeedbackNote not found.");
            }

            feedback.PlaytestSessionId =
                updatedFeedback.PlaytestSessionId;
            feedback.Category = updatedFeedback.Category;
            feedback.Comment = updatedFeedback.Comment;
            feedback.Rating = updatedFeedback.Rating;

            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Updated feedback note {FeedbackNoteId}",
                feedback.Id);

            return Results.Ok(feedback);
        });

        // DELETE a feedback note
        routes.MapDelete("/{id:int}", async (
            int id,
            PlayTestOpsDbContext db) =>
        {
            var feedback = await db.FeedbackNotes.FindAsync(id);

            if (feedback is null)
            {
                return Results.NotFound(
                    "FeedbackNote not found.");
            }

            db.FeedbackNotes.Remove(feedback);
            await db.SaveChangesAsync();

            app.Logger.LogInformation(
                "Deleted feedback note {FeedbackNoteId}",
                feedback.Id);

            return Results.NoContent();
        });
    }
}