using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Dto.Mappings;
using FitnessTrackerBackend.Dto.WorkoutPlans;
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
        public async Task<ActionResult<IEnumerable<WorkoutPlanResponse>>> GetAllWorkoutPlans(CancellationToken ct)
        {
            try
            {
                var workoutPlans = await _context.WorkoutPlans
                    .AsNoTracking()
                    .ToListAsync(ct);
                
                var workoutPlanDto = workoutPlans.Select(wp => wp.ToResponse()).ToList();

                return Ok(workoutPlanDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: Workoutplan by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutPlanDetailedResponse>> GetWorkoutPlanById(Guid id, CancellationToken ct)
        {
            try
            {
                // Get workout plan by ID, including related Exercises and User
                var workoutPlan = await _context.WorkoutPlans
                    .Include(wp => wp.Exercises)
                    .Include(wp => wp.User)
                    .FirstOrDefaultAsync(wp => wp.Id == id, ct);

                if (workoutPlan == null)
                {
                    return NotFound();
                }


                return Ok(workoutPlan.ToDetailedResponse());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: WorkoutPlan by UserID
        [HttpGet("by-user/{id}")]
        public async Task<ActionResult<IEnumerable<WorkoutPlanDetailedResponse>>> GetWorkoutPlansByUserId(Guid id, CancellationToken ct)
        {
            try
            {
                // Get workout plans by user ID, including related Exercises and User
                var workoutPlans = await _context.WorkoutPlans
                    .Include(wp => wp.Exercises)
                    .Include(wp => wp.User)
                    .Where(wp => wp.UserId == id)
                    .ToListAsync(ct);

                if (workoutPlans == null)
                {
                    return NotFound();
                }

                var workoutPlansDto = workoutPlans.Select(wp => wp.ToDetailedResponse()).ToList();

                return Ok(workoutPlansDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: Create a new WorkoutPlan
        [HttpPost]
        public async Task<ActionResult<WorkoutPlanResponse>> CreateWorkoutPlan(CreateWorkoutPlanRequest workoutPlan, CancellationToken ct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (workoutPlan.UserId == Guid.Empty)
                    {
                        ModelState.AddModelError(nameof(workoutPlan.UserId), "UserId is required.");
                    }

                    if (string.IsNullOrWhiteSpace(workoutPlan.Name))
                    {
                        ModelState.AddModelError(nameof(workoutPlan.Name), "Name is required.");
                    }

                    if (string.IsNullOrWhiteSpace(workoutPlan.Type))
                    {
                        ModelState.AddModelError(nameof(workoutPlan.Type), "Type is required.");
                    }

                    if (!ModelState.IsValid)
                    {
                        return ValidationProblem(ModelState);
                    }
                    
                    var workoutPlanDto = workoutPlan.ToModel();
                    _context.WorkoutPlans.Add(workoutPlanDto);
                    await _context.SaveChangesAsync(ct);
                    
                    var response = workoutPlanDto.ToResponse();
                    return CreatedAtAction(nameof(GetWorkoutPlanById), new { id = response.Id }, response);
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
        public async Task<ActionResult<WorkoutPlanDetailedResponse>> UpdateWorkoutPlan(Guid id, CreateWorkoutPlanRequest workoutPlan, CancellationToken ct)
        {
            var entity = await _context.WorkoutPlans.FirstOrDefaultAsync(wp => wp.Id == id, ct);

            if(entity == null)
            {
                return NotFound();
            }

            entity.Name = workoutPlan.Name;
            entity.Type = workoutPlan.Type;
            entity.Description = workoutPlan.Description;
            entity.Frequency = workoutPlan.Frequency;

            await _context.SaveChangesAsync(ct);
            return Ok(entity.ToDetailedResponse());
        }

        // DELETE: Delete a WorkoutPlan
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutPlan(Guid id, CancellationToken ct) 
        {
            try
            {
                var workoutPlan = await _context.WorkoutPlans.FindAsync(id, ct);

                if (workoutPlan == null)
                {
                    return NotFound();
                }
                
                _context.WorkoutPlans.Remove(workoutPlan);
                await _context.SaveChangesAsync(ct);
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
