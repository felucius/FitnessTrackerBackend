using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.WorkoutPlans
{
    public sealed record WorkoutPlanResponse
    (
        Guid Id,
        Guid UserId,
        User? User,
        string Name,
        string Type,
        string? Description,
        int? Frequency
    );
}
