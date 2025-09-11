using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // => /api/workoutplans
    public class WorkoutPlansController : Controller
    {
        private readonly AppDbContext _context;

        public WorkoutPlansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: WorkoutPlans
        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutPlans()
        {
            try
            {
                var workoutPlans = await _context.WorkoutPlans.AsNoTracking().ToListAsync();
                return Ok(workoutPlans);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: Workoutplan by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutPlanById(Guid id)
        {
            try
            {
                // Get workout plan by ID, including related Exercises and User
                var workoutPlan = await _context.WorkoutPlans
                    .Include(wp => wp.Exercises)
                    .Include(wp => wp.User)
                    .FirstOrDefaultAsync(wp => wp.Id == id);
                if (workoutPlan == null)
                {
                    return NotFound();
                }
                return Ok(workoutPlan);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: WorkoutPlan by UserID
        [HttpGet("by-user/{id}")]
        public async Task<IActionResult> GetWorkoutPlanByUserId(Guid id)
        {
            try
            {
                // Get workout plans by user ID, including related Exercises and User
                var workoutPlans = await _context.WorkoutPlans
                    .Include(wp => wp.Exercises)
                    .Include(wp => wp.User)
                    .Where(wp => wp.UserId == id)
                    .ToListAsync();

                if (workoutPlans == null)
                {
                    return NotFound();
                }

                return Ok(workoutPlans);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: Create a new WorkoutPlan
        [HttpPost]
        public async Task<IActionResult> CreateWorkoutPlan(WorkoutPlan workoutPlan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // If Workoutplan exercises are zero or null, initialize to empty list
                    if (workoutPlan.Exercises == null || workoutPlan.Exercises.Count == 0)
                    {
                        workoutPlan.Exercises = new List<Exercise>();
                    }

                    workoutPlan.Id = Guid.NewGuid();
                    _context.Add(workoutPlan);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetWorkoutPlanByUserId), new { id = workoutPlan.Id }, workoutPlan);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: Update an existing WorkoutPlan
        [HttpPut("{id}")]
        public async Task<ActionResult<WorkoutPlan>> UpdateWorkoutPlan(Guid id, WorkoutPlan workoutPlan)
        {
            try
            {
                if (id != workoutPlan.Id)
                {
                    return BadRequest();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        //_context.Update(workoutPlan);
                        _context.Entry(workoutPlan).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        var WorkoutPlanExists = _context.WorkoutPlans.Any(e => e.Id == workoutPlan.Id);
                        if (!WorkoutPlanExists)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    // Return the updated workout plan with related Exercises and User
                    var updatedWorkoutPlan = _context.WorkoutPlans
                        .Include(wp => wp.Exercises)
                        .Include(wp => wp.User)
                        .Where(wp => wp.Id == workoutPlan.Id)
                        .First();

                    return updatedWorkoutPlan;
                }
                return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: Delete a WorkoutPlan
        [HttpDelete("{id}")]
        public ActionResult DeleteWorkoutPlan(Guid id) 
        {
            try
            {
                var workoutPlan = _context.WorkoutPlans.Find(id);
                if (workoutPlan == null)
                {
                    return NotFound();
                }
                _context.WorkoutPlans.Remove(workoutPlan);
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
