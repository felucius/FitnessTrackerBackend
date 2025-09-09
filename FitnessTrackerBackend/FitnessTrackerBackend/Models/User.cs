
namespace FitnessTrackerBackend.Models
{
    public class User
    {
        public Guid Id { get; set; } = default!;
        public string? Gender { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int? Age { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public string? Password { get; set; }

        public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
        public ICollection<Progression> Progressions { get; set; } = new List<Progression>();
    }
}
