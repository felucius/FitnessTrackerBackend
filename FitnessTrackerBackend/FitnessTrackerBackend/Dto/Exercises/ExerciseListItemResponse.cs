using FitnessTrackerBackend.Dto.Progressions;

namespace FitnessTrackerBackend.Dto.Exercises
{
    public sealed record ExerciseListItemResponse(
        Guid UniqueExerciseId,
        string ExerciseId,
        Guid WorkoutPlanId,
        string? Name,
        string? ImageUrl,
        string? ExerciseType,
        IReadOnlyList<string> TargetMuscles,
        IReadOnlyList<string> BodyParts,
        IReadOnlyList<ExerciseProgressionResponse> Progressions
    );
}
