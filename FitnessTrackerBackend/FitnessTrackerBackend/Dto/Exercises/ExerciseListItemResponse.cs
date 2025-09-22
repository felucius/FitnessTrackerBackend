using FitnessTrackerBackend.Dto.Progressions;

namespace FitnessTrackerBackend.Dto.Exercises
{
    public sealed record ExerciseListItemResponse(
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
