namespace FitnessTrackerBackend.Models
{
    public class Dashboard
    {
        public Guid Id { get; set; } = default!;
        public Guid UserId { get; set; } = default!;
        public User User { get; set; } = default!;

        public DateTime? UpcomingWorkout { get; set; }

        // Stored as JSON arrays
        public string? ExercisesToPerformJson { get; set; }
        public string? PersonalRecordsJson { get; set; }
    }
}
