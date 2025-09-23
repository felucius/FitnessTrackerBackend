using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Progressions
{
    public sealed record CreateExerciseProgressionRequest
    (
        Guid UniqueExerciseId,
        Guid? UserId,
        string ExerciseId,
        DateTime Date,
        int Weight,
        int Reps
    );
}
