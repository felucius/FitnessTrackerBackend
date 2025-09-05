namespace FitnessTrackerBackend.Models
{
    public class Exercise
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? Instructions { get; set; }

        public ICollection<Progression> Progressions { get; set; } = new List<Progression>();
    }
}
