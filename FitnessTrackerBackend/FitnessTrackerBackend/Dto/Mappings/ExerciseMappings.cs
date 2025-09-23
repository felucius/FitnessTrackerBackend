using FitnessTrackerBackend.Dto.Exercises;
using FitnessTrackerBackend.Dto.Progressions;
using FitnessTrackerBackend.Models;
using System.Linq.Expressions;

namespace FitnessTrackerBackend.Dto.Mappings
{
    public static class ExerciseMappings
    {
        // Server decides ExerciseId here. If you prefer client-supplied IDs, accept it in Create DTO and remove the generation.
        public static Exercise FromCreate(this CreateExerciseRequest r) => new Exercise
        {
            ExerciseId = r.ExerciseId,
            WorkoutPlanId = r.WorkoutPlanId,
            Name = TrimOrNull(r.Name),
            ImageUrl = r.ImageUrl,
            Equipments = r.Equipments?.ToArray() ?? Array.Empty<string>(),
            BodyParts = r.BodyParts?.ToArray() ?? Array.Empty<string>(),
            ExerciseType = r.ExerciseType,
            TargetMuscles = r.TargetMuscles?.ToArray() ?? Array.Empty<string>(),
            SecondaryMuscles = r.SecondaryMuscles?.ToArray() ?? Array.Empty<string>(),
            VideoUrl = r.VideoUrl,
            Keywords = r.Keywords?.ToArray() ?? Array.Empty<string>(),
            Overview = r.Overview,
            Instructions = r.Instructions?.ToArray() ?? Array.Empty<string>(),
            ExerciseTips = r.ExerciseTips?.ToArray() ?? Array.Empty<string>(),
            Variations = r.Variations?.ToArray() ?? Array.Empty<string>(),
            RelatedExerciseIds = r.RelatedExerciseIds?.ToArray() ?? Array.Empty<string>()
        };

        public static void ApplyUpdate(this Exercise e, UpdateExerciseRequest r)
        {
            e.Name = TrimOrNull(r.Name);
            e.ImageUrl = r.ImageUrl;
            e.Equipments = r.Equipments?.ToArray() ?? Array.Empty<string>();
            e.BodyParts = r.BodyParts?.ToArray() ?? Array.Empty<string>();
            e.ExerciseType = r.ExerciseType;
            e.TargetMuscles = r.TargetMuscles?.ToArray() ?? Array.Empty<string>();
            e.SecondaryMuscles = r.SecondaryMuscles?.ToArray() ?? Array.Empty<string>();
            e.VideoUrl = r.VideoUrl;
            e.Keywords = r.Keywords?.ToArray() ?? Array.Empty<string>();
            e.Overview = r.Overview;
            e.Instructions = r.Instructions?.ToArray() ?? Array.Empty<string>();
            e.ExerciseTips = r.ExerciseTips?.ToArray() ?? Array.Empty<string>();
            e.Variations = r.Variations?.ToArray() ?? Array.Empty<string>();
            e.RelatedExerciseIds = r.RelatedExerciseIds?.ToArray() ?? Array.Empty<string>();
        }

        public static ExerciseResponse ToResponse(this Exercise e) => new ExerciseResponse(
            e.ExerciseId,
            e.WorkoutPlanId,
            e.Name,
            e.ImageUrl,
            e.Equipments ?? Array.Empty<string>(),
            e.BodyParts ?? Array.Empty<string>(),
            e.ExerciseType,
            e.TargetMuscles ?? Array.Empty<string>(),
            e.SecondaryMuscles ?? Array.Empty<string>(),
            e.VideoUrl,
            e.Keywords ?? Array.Empty<string>(),
            e.Overview,
            e.Instructions ?? Array.Empty<string>(),
            e.ExerciseTips ?? Array.Empty<string>(),
            e.Variations ?? Array.Empty<string>(),
            e.RelatedExerciseIds ?? Array.Empty<string>(),
            e.Progressions?.Count ?? 0
        );

        // EF-friendly projections for List & Detail
        public static readonly Expression<Func<Exercise, ExerciseListItemResponse>> ToListItemExpr =
            e => new ExerciseListItemResponse(
                e.ExerciseId,
                e.WorkoutPlanId,
                e.Name,
                e.ImageUrl,
                e.ExerciseType,
                e.TargetMuscles ?? Array.Empty<string>(),
                e.BodyParts ?? Array.Empty<string>(),
                e.Progressions
                    .OrderByDescending(p => p.Date)
                    .Select(p => new ExerciseProgressionResponse(
                        p.Id,
                        p.Id,
                        e.ExerciseId,
                        null,
                        p.Date,
                        p.Weight,
                        p.Reps
                    ))
            .ToList());

        public static readonly Expression<Func<Exercise, ExerciseResponse>> ToResponseExpr =
            e => new ExerciseResponse(
                e.ExerciseId,
                e.WorkoutPlanId,
                e.Name,
                e.ImageUrl,
                e.Equipments ?? Array.Empty<string>(),
                e.BodyParts ?? Array.Empty<string>(),
                e.ExerciseType,
                e.TargetMuscles ?? Array.Empty<string>(),
                e.SecondaryMuscles ?? Array.Empty<string>(),
                e.VideoUrl,
                e.Keywords ?? Array.Empty<string>(),
                e.Overview,
                e.Instructions ?? Array.Empty<string>(),
                e.ExerciseTips ?? Array.Empty<string>(),
                e.Variations ?? Array.Empty<string>(),
                e.RelatedExerciseIds ?? Array.Empty<string>(),
                e.Progressions != null ? e.Progressions.Count : 0);

        private static string? TrimOrNull(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
    }
}