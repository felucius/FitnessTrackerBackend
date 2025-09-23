
using FitnessTrackerBackend.Dto.Dashboard;
using FitnessTrackerBackend.Dto.Exercises;
using FitnessTrackerBackend.Dto.Progressions;
using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Mappings
{
    public static class DashboardMappings
    {
        public static DashboardResponse ToResponse(this Models.Dashboard dashboard)
        {
            var response = new DashboardResponse(
                User: dashboard.User,
                WorkoutPlan: dashboard.WorkoutPlan,
                UpcomingWorkout: dashboard.UpcomingWorkout,
                Exercises: (dashboard.Exercises ?? new List<Exercise>())
                    .Select(e => new ExerciseListItemResponse(
                        e.ExerciseId,
                        e.WorkoutPlanId,
                        e.Name,
                        e.ImageUrl,
                        e.ExerciseType,
                        e.TargetMuscles ?? Array.Empty<string>(),
                        e.BodyParts ?? Array.Empty<string>(),
                        (e.Progressions ?? Enumerable.Empty<Progression>())
                            .Where(p => p.UserId == dashboard.User.Id)
                            .OrderByDescending(p => p.Date)
                            .Select(p => new ExerciseProgressionResponse(p.Id, p.Id, e.ExerciseId, null, p.Date, p.Weight, p.Reps))
                            .ToList()
                            ))
                    .ToList()
            );

            return response;
        }
    }
}
