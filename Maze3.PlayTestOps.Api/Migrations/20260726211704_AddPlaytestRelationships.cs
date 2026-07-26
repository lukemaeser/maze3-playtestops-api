using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maze3.PlayTestOps.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaytestRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PlaytestSessions_GameBuildId",
                table: "PlaytestSessions",
                column: "GameBuildId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackNotes_PlaytestSessionId",
                table: "FeedbackNotes",
                column: "PlaytestSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_BugReports_PlaytestSessionId",
                table: "BugReports",
                column: "PlaytestSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BugReports_PlaytestSessions_PlaytestSessionId",
                table: "BugReports",
                column: "PlaytestSessionId",
                principalTable: "PlaytestSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackNotes_PlaytestSessions_PlaytestSessionId",
                table: "FeedbackNotes",
                column: "PlaytestSessionId",
                principalTable: "PlaytestSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaytestSessions_GameBuilds_GameBuildId",
                table: "PlaytestSessions",
                column: "GameBuildId",
                principalTable: "GameBuilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BugReports_PlaytestSessions_PlaytestSessionId",
                table: "BugReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackNotes_PlaytestSessions_PlaytestSessionId",
                table: "FeedbackNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaytestSessions_GameBuilds_GameBuildId",
                table: "PlaytestSessions");

            migrationBuilder.DropIndex(
                name: "IX_PlaytestSessions_GameBuildId",
                table: "PlaytestSessions");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackNotes_PlaytestSessionId",
                table: "FeedbackNotes");

            migrationBuilder.DropIndex(
                name: "IX_BugReports_PlaytestSessionId",
                table: "BugReports");
        }
    }
}
