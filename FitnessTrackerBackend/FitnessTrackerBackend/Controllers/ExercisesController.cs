using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Dto.Exercises;
using FitnessTrackerBackend.Dto.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FitnessTrackerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExercisesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseListItemResponse>>> GetAllExercises(CancellationToken ct)
        {
            try
            {
                var exercisesDto = await _context.Exercises
                    .OrderBy(ex => ex.Name)
                    .Select(ExerciseMappings.ToResponseExpr)
                    .ToListAsync(ct);

                return Ok(exercisesDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/exercises/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseResponse>> GetExerciseById(string id, CancellationToken ct)
        {
            try
            {
                var exerciseDto = await _context.Exercises
                    .Where(ex => ex.ExerciseId == id)
                    .Select(ExerciseMappings.ToResponseExpr)
                    .SingleOrDefaultAsync(ct);

                if (exerciseDto == null)
                {
                    return NotFound();
                }
                
                return Ok(exerciseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //POST: api/exercises
        [HttpPost]
        public async Task<ActionResult<ExerciseResponse>> CreateExercise(CreateExerciseRequest exercise, CancellationToken ct)
        {

            if (string.IsNullOrWhiteSpace(exercise.ExerciseId))
            {
                ModelState.AddModelError(nameof(exercise.ExerciseId), "ExerciseId is required.");
            }
            
            if (exercise.WorkoutPlanId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(exercise.WorkoutPlanId), "WorkoutPlanId is required.");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var exists = await _context.Exercises.AnyAsync(e => e.ExerciseId == exercise.ExerciseId, ct);
            
            if (exists)
            {
                return Conflict(new { title = "Exercise already exists" });
            }

            // maps DTO -> entity (uses client ExerciseId)
            var entity = exercise.FromCreate();    
            _context.Exercises.Add(entity);
            await _context.SaveChangesAsync(ct);

            // entity -> DTO
            var response = entity.ToResponse(); 
            return CreatedAtAction(nameof(GetExerciseById), new { id = response.ExerciseId }, response);
        }

        // DELETE: api/exercises/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(string id, CancellationToken ct)
        {
            try
            {
                var exercise = await _context.Exercises.FindAsync(id, ct);

                if (exercise == null)
                {
                    return NotFound();
                }
                
                _context.Exercises.Remove(exercise);
                _context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
