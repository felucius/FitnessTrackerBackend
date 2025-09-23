using FitnessTrackerBackend.Dto.Exercises;
using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Dashboard
{
    public sealed record DashboardResponse
    (
        User User,
        WorkoutPlan WorkoutPlan,
        DateTimeOffset? UpcomingWorkout,
        IReadOnlyList<ExerciseListItemResponse> Exercises
    );
}
