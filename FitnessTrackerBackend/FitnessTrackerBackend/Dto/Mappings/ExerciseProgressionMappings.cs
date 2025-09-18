using FitnessTrackerBackend.Dto.Progressions;
using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Mappings
{
    public static class ExerciseProgressionMappings
    {
        public static ExerciseProgressionResponse ToResponse(this Progression progression)
        {
            var response = new ExerciseProgressionResponse(
                Id: progression.Id,
                UserId: progression.UserId,
                ExerciseId: progression.ExerciseId,
                Exercise: progression.Exercise,
                Date: progression.Date,
                Weight: progression.Weight,
                Reps: progression.Reps
            );
            return response;
        }

        public static Progression ToModel(this CreateExerciseProgressionRequest request)
        {
            var model = new Progression
            {
                UserId = (Guid)request.UserId,
                ExerciseId = request.ExerciseId,
                Date = request.Date,
                Weight = request.Weight,
                Reps = request.Reps
            };
            return model;
        }
    }
}
