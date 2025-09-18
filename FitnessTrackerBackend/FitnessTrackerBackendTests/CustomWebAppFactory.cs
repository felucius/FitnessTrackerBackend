using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTrackerBackendTests
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private SqliteConnection _connection = default!;

        public Task InitializeAsync()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            return _connection.OpenAsync();
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            _connection.Dispose();
            return Task.CompletedTask;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app’s real DbContext registration
                var toRemove = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (toRemove is not null) services.Remove(toRemove);

                // Use SQLite in-memory (connection kept open for the test run)
                //services.AddDbContext<AppDbContext>(opts => opts.UseSqlite(_connection));
                services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("tests"));


                // Build the provider and ensure DB is created + seeded
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                Seed(db);
            });
        }

        private static void Seed(AppDbContext db)
        {
            // Seed only what UsersController needs
            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User
                    {
                        Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        Name = "Alice",
                        Email = "alice@example.com",
                        Gender = "F",
                    },
                    new User
                    {
                        Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                        Name = "Bob",
                        Email = "bob@example.com",
                        Gender = "M",
                    }
                );
                db.SaveChanges();
            }

            if (!db.WorkoutPlans.Any())
            {
                db.WorkoutPlans.AddRange(
                    new WorkoutPlan
                    {
                        Id = Guid.Parse("caaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        Name = "Full body workout",
                        Description = "Training for endurance",
                        Exercises = new List<Exercise>(),
                        Frequency = 2,
                        Type = "Cardio",
                        User = new User
                        {
                            Name = "Alice",
                            Email = "alice@example.com",
                            Age = 22,
                            Gender = "Female",
                            Height = 167
                        },
                        UserId = new Guid(),
                    }
                );

                db.SaveChanges();
            }
        }
    }
}
