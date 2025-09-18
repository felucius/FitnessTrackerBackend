using FitnessTrackerBackend.Dto.Exercises;
using FitnessTrackerBackend.Dto.Mappings;
using FitnessTrackerBackend.Dto.WorkoutPlans;
using FitnessTrackerBackend.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrackerBackendTests
{
    public class WorkoutPlansTests
    {
        [Fact]
        public void WorkoutPlans_ToResponse_RetrieveUserId()
        {
            var workoutPlan = new WorkoutPlan
            {
                Id = new Guid(),
                Name = "Upper body",
                Description = "Strength building",
                Frequency = 2,
                Type = "strength",
                UserId = new Guid()
            };

            var dto = workoutPlan.ToResponse();

            dto.Id.Should().Subject.Should().HaveValue();
            dto.UserId.Should().Subject.Should().HaveValue();

            typeof(WorkoutPlanResponse).GetProperties().Select(w => w.Name).Should().Contain("UserId");
        }

        [Fact]
        public void WorkoutPlans_TeDetailedResponse_RetrieveFullObject()
        {
            var workoutPlan = new WorkoutPlan
            {
                Id = new Guid(),
                Name = "Full body",
                Type = "Strength",
                UserId = new Guid(),
                Exercises = new List<Exercise>()
                {
                    new Exercise {
                        ExerciseId = "randomExerciseId",
                        WorkoutPlanId = new Guid(),
                        Name = "Chest press",
                        ImageUrl = "image url",
                        Equipments = new string[2],
                        BodyParts = new string[4],
                        ExerciseType = string.Empty,
                        TargetMuscles = new string[2],
                        SecondaryMuscles = new string[2],
                        VideoUrl = "VideoUrl",
                        Keywords = new string[7],
                        Overview = "overview",
                        Instructions = new string[2],
                        ExerciseTips = new string[9],
                        Variations = new string[1],
                        RelatedExerciseIds = null
                    }
                },
                Frequency = 2,
                Description = "Build strength"
            };

            var dto = workoutPlan.ToDetailedResponse();

            dto.Exercises.Should().NotBeNull();
            dto.Exercises.Should().Subject.Select(x => x.ExerciseId.Should().NotBeNull());
            dto.Exercises.Should().Subject.Select(x => x.BodyParts.Count > 3);
            dto.Name.Should().Subject.Should().Contain("Full body");
        }
    }
}
