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
            var exercises = _context.Exercises.ToList();
            return Ok(exercises);
        }

        // GET: api/exercises/{id}
        [HttpGet("{id}")]
        public IActionResult GetExerciseById(Guid id)
        {
            var exercise = _context.Exercises.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return Ok(exercise);
        }

        // POST: api/exercises
        [HttpPost]
        public IActionResult CreateExercise(Exercise exercise)
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
    }
}
