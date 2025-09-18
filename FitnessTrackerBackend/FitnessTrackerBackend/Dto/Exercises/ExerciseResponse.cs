namespace FitnessTrackerBackend.Dto.Exercises
{
    public sealed record ExerciseResponse(
        string ExerciseId,
        Guid WorkoutPlanId,
        string? Name,
        string? ImageUrl,
        IReadOnlyList<string> Equipments,
        IReadOnlyList<string> BodyParts,
        string? ExerciseType,
        IReadOnlyList<string> TargetMuscles,
        IReadOnlyList<string> SecondaryMuscles,
        string? VideoUrl,
        IReadOnlyList<string> Keywords,
        string? Overview,
        IReadOnlyList<string> Instructions,
        IReadOnlyList<string> ExerciseTips,
        IReadOnlyList<string> Variations,
        IReadOnlyList<string> RelatedExerciseIds,
        int ProgressionCount
    );
}
