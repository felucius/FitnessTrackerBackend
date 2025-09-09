namespace FitnessTrackerBackend.Models
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public Guid WorkoutPlanId { get; set; }
        public WorkoutPlan? WorkoutPlan { get; set; }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public bool AllDay { get; set; }
    }
}
