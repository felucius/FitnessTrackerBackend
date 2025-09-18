using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.WorkoutPlans
{
    public sealed record CreateWorkoutPlanRequest
    (
        Guid UserId,
        User? User,
        string Name,
        string Type,
        string? Description,
        int? Frequency
    );
}
