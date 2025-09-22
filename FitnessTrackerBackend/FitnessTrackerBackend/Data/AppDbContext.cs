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
                e.HasKey(x => x.ExerciseId);
                e.Property(x => x.ExerciseId).IsRequired().HasMaxLength(100);
                e.Property(x => x.Name);
                e.Property(x => x.ImageUrl);
                e.Property(x => x.Equipments).HasColumnType("nvarchar(max)");
                e.Property(x => x.BodyParts).HasColumnType("nvarchar(max)");
                e.Property(x => x.ExerciseType);
                e.Property(x => x.TargetMuscles).HasColumnType("nvarchar(max)");
                e.Property(x => x.SecondaryMuscles).HasColumnType("nvarchar(max)");
                e.Property(x => x.VideoUrl);
                e.Property(x => x.Keywords).HasColumnType("nvarchar(max)");
                e.Property(x => x.Overview).HasColumnType("nvarchar(max)");
                e.Property(x => x.Instructions).HasColumnType("nvarchar(max)");
                e.Property(x => x.ExerciseTips).HasColumnType("nvarchar(max)");
                e.Property(x => x.Variations).HasColumnType("nvarchar(max)");
                e.Property(x => x.RelatedExerciseIds).HasColumnType("nvarchar(max)");
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
                e.HasMany(x => x.Exercises)
                 .WithOne(x => x.WorkoutPlan)
                 .HasForeignKey(x => x.WorkoutPlanId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Calendar
            b.Entity<CalendarEvent>(e =>
            {
                e.ToTable("Calendar");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasDefaultValueSql("NEWID()");
                e.Property(x => x.Title).IsRequired().HasMaxLength(200);
                e.Property(x => x.Start).IsRequired();
                e.Property(x => x.End).IsRequired();
                
                e.HasOne(x => x.WorkoutPlan)
                 .WithMany(p => p.CalendarEvents)
                 .HasForeignKey(x => x.WorkoutPlanId)
                 .OnDelete(DeleteBehavior.Cascade);

            });

            // Progression
            b.Entity<Progression>(e =>
            {
                e.ToTable("Progression");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasDefaultValueSql("NEWID()");

                e.Property(x => x.ExerciseId).IsRequired().HasMaxLength(100);

                e.HasOne(x => x.User)
                 .WithMany(u => u.Progressions)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(x => x.Exercise)
                 .WithMany(ex => ex.Progressions)
                 .HasForeignKey(x => x.ExerciseId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.Date).HasColumnType("datetime2(3)");
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