namespace FitnessTrackerBackend.Models
{
    public class WorkoutPlan
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? Description { get; set; }
        public int? Frequency { get; set; } = default!;

        public ICollection<Exercise>? Exercises { get; set; } = [];

        public ICollection<CalendarEvent>? CalendarEvents { get; set; } = [];
    }
}
