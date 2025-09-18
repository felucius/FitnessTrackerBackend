using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Progressions
{
    public sealed record CreateExerciseProgressionRequest
    (
        Guid? UserId,
        string ExerciseId,
        DateTime Date,
        int Weight,
        int Reps
    );
}
