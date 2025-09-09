namespace FitnessTrackerBackend.Models
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public Guid WorkoutPlanId { get; set; } = default!;
        public WorkoutPlan WorkoutPlan { get; set; } = default!;

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public bool AllDay { get; set; }
    }
}
