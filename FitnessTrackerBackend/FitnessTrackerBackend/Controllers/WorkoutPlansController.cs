using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Models;

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
            var workoutPlans = await _context.WorkoutPlans.AsNoTracking().ToListAsync();
            return Ok(workoutPlans);
        }

        // GET: WorkoutPlan by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutPlanById(Guid id)
        {
            var workoutPlan = await _context.WorkoutPlans
                .Include(wp => wp.User)
                .FirstOrDefaultAsync(wp => wp.Id == id);

            if (workoutPlan == null)
            {
                return NotFound();
            }

            return Ok(workoutPlan);
        }

        // POST: Create a new WorkoutPlan
        [HttpPost]
        public async Task<IActionResult> CreateWorkoutPlan(WorkoutPlan workoutPlan)
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
                return CreatedAtAction(nameof(GetWorkoutPlanById), new { id = workoutPlan.Id }, workoutPlan);
            }

            return BadRequest(ModelState);
        }
    }
}
