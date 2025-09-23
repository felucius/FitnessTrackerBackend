namespace FitnessTrackerBackend.Models
{
    public class Dashboard
    {
        public User User { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; }
        public DateTimeOffset? UpcomingWorkout { get; set; }
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
