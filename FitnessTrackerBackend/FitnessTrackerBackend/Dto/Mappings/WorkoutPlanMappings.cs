using FitnessTrackerBackend.Dto.Exercises;
using FitnessTrackerBackend.Dto.Progressions;
using FitnessTrackerBackend.Dto.WorkoutPlans;
using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Mappings
{
    public static class WorkoutPlanMappings
    {
        public static WorkoutPlanResponse ToResponse(this WorkoutPlan wp)
        {
            var response = new WorkoutPlanResponse
            (
                Id: wp.Id,
                UserId: wp.UserId,
                User: wp.User,
                Name: wp.Name,
                Type:  wp.Type,
                Description: wp.Description,
                Frequency:  wp.Frequency
            );

            return response;
        }
        public static WorkoutPlan ToModel(this CreateWorkoutPlanRequest req)
        {
            var response = new WorkoutPlan
            {
                UserId = req.UserId,
                User = req.User,
                Name = req.Name,
                Type = req.Type,
                Description = req.Description,
                Frequency = req.Frequency
            };

            return response;
        }

        public static WorkoutPlanDetailedResponse ToDetailedResponse(this WorkoutPlan wp)
        {
            var response = new WorkoutPlanDetailedResponse(
                Id: wp.Id,
                UserId: wp.UserId,
                User: wp.User,
                Name: wp.Name,
                Type: wp.Type,
                Description: wp.Description,
                Frequency: wp.Frequency,
                Exercises: (wp.Exercises ?? new List<Exercise>())
                    .Select(e => new ExerciseListItemResponse(
                        e.ExerciseId,
                        e.WorkoutPlanId,
                        e.Name,
                        e.ImageUrl,
                        e.ExerciseType,
                        e.TargetMuscles ?? Array.Empty<string>(),
                        e.BodyParts ?? Array.Empty<string>(),
                        (e.Progressions ?? Enumerable.Empty<Progression>())
                            .Where(p => p.UserId == wp.UserId)
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
