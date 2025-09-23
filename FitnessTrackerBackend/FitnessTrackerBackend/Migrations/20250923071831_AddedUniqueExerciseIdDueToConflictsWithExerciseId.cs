using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTrackerBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueExerciseIdDueToConflictsWithExerciseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Progression_Exercises_ExerciseId",
                table: "Progression");

            migrationBuilder.DropIndex(
                name: "IX_Progression_ExerciseId",
                table: "Progression");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "ExerciseId",
                table: "Progression",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<Guid>(
                name: "UniqueExerciseId",
                table: "Progression",
                type: "uniqueidentifier",
                maxLength: 100,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UniqueExerciseId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "UniqueExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Progression_UniqueExerciseId",
                table: "Progression",
                column: "UniqueExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Progression_Exercises_UniqueExerciseId",
                table: "Progression",
                column: "UniqueExerciseId",
                principalTable: "Exercises",
                principalColumn: "UniqueExerciseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Progression_Exercises_UniqueExerciseId",
                table: "Progression");

            migrationBuilder.DropIndex(
                name: "IX_Progression_UniqueExerciseId",
                table: "Progression");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "UniqueExerciseId",
                table: "Progression");

            migrationBuilder.DropColumn(
                name: "UniqueExerciseId",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "ExerciseId",
                table: "Progression",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Progression_ExerciseId",
                table: "Progression",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Progression_Exercises_ExerciseId",
                table: "Progression",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "ExerciseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
