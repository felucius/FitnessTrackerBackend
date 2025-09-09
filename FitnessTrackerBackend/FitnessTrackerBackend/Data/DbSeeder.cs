using FitnessTrackerBackend.Models;

namespace FitnessTrackerBackend.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User { Id = new Guid(), Gender = "Male", Name = "Maxime de Lange", Email = "maxime@testmail.com", Age = 34, Weight = 88, Height = 184, Password = "" },
                    new User { Id = new Guid(), Gender = "Female", Name = "Emily Smith", Email = "emily@testmail.com", Age = 32, Weight = 62, Height = 165, Password = "" }
                );
            }

            if (!db.Exercises.Any())
            {
                db.Exercises.AddRange(
                    new Exercise { Id = new Guid(), Name = "Push-up", Type = "Strength", Instructions = "Start in a plank..." },
                    new Exercise { Id = new Guid(), Name = "Squat", Type = "Strength", Instructions = "Stand with feet..." }
                );
            }

            await db.SaveChangesAsync();
        }
    }
}
