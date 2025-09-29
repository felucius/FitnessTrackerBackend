namespace FitnessTrackerBackend.Models
{
    public class Exercise
    {
        public Guid UniqueExerciseId { get; set; } = default!;
        public string ExerciseId { get; set; } = default!;

        public Guid WorkoutPlanId { get; set; }
        public WorkoutPlan? WorkoutPlan { get; set; }

        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string[]? Equipments { get; set; }
        public string[]? BodyParts { get; set; }
        public string? ExerciseType { get; set; }
        public string[]? TargetMuscles { get; set; }
        public string[]? SecondaryMuscles { get; set; }
        public string? VideoUrl { get; set; }
        public string[]? Keywords { get; set; }
        public string? Overview { get; set; }
        public string[]? Instructions { get; set; }
        public string[]? ExerciseTips { get; set; }
        public string[]? Variations { get; set; }
        public string[]? RelatedExerciseIds { get; set; }


        public ICollection<Progression>? Progressions { get; set; } = new List<Progression>();
    }
}
