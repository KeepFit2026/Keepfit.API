using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeepFit.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FitnessProgram",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MuscleGroupId",
                table: "Exercise",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "MuscleGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    DayIndex = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seance_FitnessProgram_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "FitnessProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeanceExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Reps = table.Column<int>(type: "int", nullable: true),
                    Break = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeanceExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeanceExercise_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeanceExercise_Seance_SeanceId",
                        column: x => x.SeanceId,
                        principalTable: "Seance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_MuscleGroupId",
                table: "Exercise",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Seance_ProgramId",
                table: "Seance",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_SeanceExercise_ExerciseId",
                table: "SeanceExercise",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_SeanceExercise_SeanceId",
                table: "SeanceExercise",
                column: "SeanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_MuscleGroup_MuscleGroupId",
                table: "Exercise",
                column: "MuscleGroupId",
                principalTable: "MuscleGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_MuscleGroup_MuscleGroupId",
                table: "Exercise");

            migrationBuilder.DropTable(
                name: "MuscleGroup");

            migrationBuilder.DropTable(
                name: "SeanceExercise");

            migrationBuilder.DropTable(
                name: "Seance");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_MuscleGroupId",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FitnessProgram");

            migrationBuilder.DropColumn(
                name: "MuscleGroupId",
                table: "Exercise");
        }
    }
}
