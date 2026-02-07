using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeepFit.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDifficultyFromExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Exercise",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Exercise");
        }
    }
}
