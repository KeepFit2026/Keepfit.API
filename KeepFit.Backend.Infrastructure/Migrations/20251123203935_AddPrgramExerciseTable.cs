using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeepFit.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrgramExerciseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgramExercises",
                columns: table => new
                {
                    ProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramExercises", x => new { x.ProgramId, x.ExerciseId });
                    table.ForeignKey(
                        name: "FK_ProgramExercises_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramExercises_FitnessProgram_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "FitnessProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramExercises_ExerciseId",
                table: "ProgramExercises",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramExercises");
        }
    }
}
