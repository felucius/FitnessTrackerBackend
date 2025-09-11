using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Models;
using Microsoft.AspNetCore.Mvc;


namespace FitnessTrackerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExercisesController : Controller
    {
        private readonly AppDbContext _context;

        public ExercisesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/exercises
        [HttpGet]
        public IActionResult GetAllExercises()
        {
            try
            {
                var exercises = _context.Exercises.ToList();
                return Ok(exercises);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/exercises/{id}
        [HttpGet("{id}")]
        public IActionResult GetExerciseById(Guid id)
        {
            try
            {
                var exercise = _context.Exercises.Find(id);
                if (exercise == null)
                {
                    return NotFound();
                }
                return Ok(exercise);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/exercises
        [HttpPost]
        public IActionResult CreateExercise(Exercise exercise)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //exercise.ExerciseId = Guid.NewGuid();
                    _context.Exercises.Add(exercise);
                    _context.SaveChanges();
                    return CreatedAtAction(nameof(GetExerciseById), new { id = exercise.ExerciseId }, exercise);
                }
                return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/exercises/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteExercise(string id)
        {
            try
            {
                var exercise = _context.Exercises.Find(id);
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
