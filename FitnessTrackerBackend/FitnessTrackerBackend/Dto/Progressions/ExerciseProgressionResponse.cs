using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Dto.Progressions
{
    public sealed record ExerciseProgressionResponse
    (
        Guid Id,
        Guid? UserId,
        string ExerciseId,
        Exercise Exercise,
        DateTime Date,
        int Weight,
        int Reps
    );
}
