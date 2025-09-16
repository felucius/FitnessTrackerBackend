using FitnessTrackerBackend.Dto.Exercises;
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
    // WIP: need more fields for workoutplan details
    );
}
