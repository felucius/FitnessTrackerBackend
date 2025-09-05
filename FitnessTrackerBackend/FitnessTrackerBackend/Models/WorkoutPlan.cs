namespace FitnessTrackerBackend.Models
{
    public class WorkoutPlan
    {
        public string Id { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;

        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? Description { get; set; }
        public int? Frequency { get; set; }

        // Stored as JSON (NVARCHAR(MAX))
        public string? ExerciseIdsJson { get; set; }
        public string? ExercisesJson { get; set; }

        public ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
    }
}
