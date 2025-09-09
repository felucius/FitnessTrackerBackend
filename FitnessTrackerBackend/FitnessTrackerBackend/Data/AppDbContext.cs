using FitnessTrackerBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerBackend.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Exercise> Exercises => Set<Exercise>();
        public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
        public DbSet<CalendarEvent> Calendar => Set<CalendarEvent>();
        public DbSet<Progression> Progression => Set<Progression>();
        public DbSet<Dashboard> Dashboard => Set<Dashboard>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // Users
            b.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasDefaultValueSql("NEWID()");
                e.Property(x => x.Name).IsRequired().HasMaxLength(200);
                e.Property(x => x.Email).IsRequired().HasMaxLength(256);
                e.Property(x => x.Gender).HasMaxLength(20);
                e.Property(x => x.Password).HasMaxLength(255);
            });

            // Exercises
            b.Entity<Exercise>(e =>
            {
                e.ToTable("Exercises");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasDefaultValueSql("NEWID()");
                e.Property(x => x.Name).IsRequired().HasMaxLength(200);
                e.Property(x => x.Type).IsRequired().HasMaxLength(50);
                e.Property(x => x.Instructions).HasColumnType("nvarchar(max)");
            });

            // WorkoutPlans
            b.Entity<WorkoutPlan>(e =>
            {
                e.ToTable("WorkoutPlans");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasDefaultValueSql("NEWID()");
                e.Property(x => x.UserId).IsRequired();
                e.HasOne(x => x.User)
                 .WithMany(u => u.WorkoutPlans)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.Name).IsRequired().HasMaxLength(200);
                e.Property(x => x.Type).IsRequired().HasMaxLength(100);
                e.Property(x => x.Description).HasColumnType("nvarchar(max)");
                e.Property(x => x.Frequency);

                // JSON string columns
                e.Property(x => x.ExerciseIdsJson).HasColumnType("nvarchar(max)");
                e.Property(x => x.ExercisesJson).HasColumnType("nvarchar(max)");
            });

            // Calendar
            b.Entity<CalendarEvent>(e =>
            {
                e.ToTable("Calendar");
                e.HasKey(x => x.Id);
                e.HasOne(x => x.WorkoutPlan)
                 .WithMany(p => p.CalendarEvents)
                 .HasForeignKey(x => x.WorkoutPlanId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.Title).IsRequired().HasMaxLength(200);
                e.Property(x => x.Start).IsRequired();
                e.Property(x => x.End).IsRequired();
            });

            // Progression
            b.Entity<Progression>(e =>
            {
                e.ToTable("Progression");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasDefaultValueSql("NEWID()");

                e.HasOne(x => x.User)
                 .WithMany(u => u.Progressions)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Exercise)
                 .WithMany(ex => ex.Progressions)
                 .HasForeignKey(x => x.ExerciseId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.Date).HasColumnType("date");
            });

            // Dashboard
            b.Entity<Dashboard>(e =>
            {
                e.ToTable("Dashboard");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasDefaultValueSql("NEWID()");
                e.Property(x => x.UserId).IsRequired();

                e.HasOne(x => x.User)
                 .WithMany() // no back-collection on User for dashboards
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.ExercisesToPerformJson).HasColumnType("nvarchar(max)");
                e.Property(x => x.PersonalRecordsJson).HasColumnType("nvarchar(max)");
            });
        }
    }
}