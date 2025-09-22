using FitnessTrackerBackend.Dto.Exercises;
using FitnessTrackerBackend.Dto.Progressions;
using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.WorkoutPlans
{
    public sealed record WorkoutPlanDetailedResponse
    (
        Guid Id,
        Guid UserId,
        User? User,
        string Name,
        string Type,
        string? Description,
        int? Frequency,
        IReadOnlyList<ExerciseListItemResponse> Exercises
    );
}
