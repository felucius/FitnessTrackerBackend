namespace FitnessTrackerBackend.Models
{
    public class Progression
    {
        public Guid Id { get; set; } = default!;
        public Guid UserId { get; set; } = default!;
        public User User { get; set; } = default!;

        public Guid ExerciseId { get; set; } = default!;
        public Exercise Exercise { get; set; } = default!;

        public DateTime Date { get; set; }
        public int Weight { get; set; }
        public int Reps { get; set; }
    }
}
