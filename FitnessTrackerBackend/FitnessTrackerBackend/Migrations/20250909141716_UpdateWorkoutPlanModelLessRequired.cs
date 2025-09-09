using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTrackerBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkoutPlanModelLessRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExercisesJson",
                table: "WorkoutPlans",
                newName: "Exercises");

            migrationBuilder.RenameColumn(
                name: "ExerciseIdsJson",
                table: "WorkoutPlans",
                newName: "ExerciseIds");

            migrationBuilder.AlterColumn<int>(
                name: "Frequency",
                table: "WorkoutPlans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Exercises",
                table: "WorkoutPlans",
                newName: "ExercisesJson");

            migrationBuilder.RenameColumn(
                name: "ExerciseIds",
                table: "WorkoutPlans",
                newName: "ExerciseIdsJson");

            migrationBuilder.AlterColumn<int>(
                name: "Frequency",
                table: "WorkoutPlans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
