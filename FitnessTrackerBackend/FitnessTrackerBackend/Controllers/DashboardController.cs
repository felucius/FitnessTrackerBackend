using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Dto.Dashboard;
using FitnessTrackerBackend.Dto.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerBackend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context) 
        { 
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDashboardMetricsByUserId(Guid id, CancellationToken ct)
        {
            try
            {
                DashboardResponse response = null;

                // Find user by user id
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                // Find workoutplan by user id
                var workout = await _context.WorkoutPlans
                    .Include(wp => wp.Exercises)
                        .ThenInclude(x => x.Progressions.Where(u => u.UserId == id)
                        .OrderBy(x => x.Date))
                    .Include(wp => wp.CalendarEvents)
                    .FirstOrDefaultAsync(x => x.UserId == id, ct);

                if (workout == null)
                {
                    return NotFound();
                }

                if (workout.CalendarEvents.Any())
                {
                    // Get the first calendar event based on date.
                    var upcomingWorkout = workout.CalendarEvents?.OrderBy(x => x.Start).FirstOrDefault();

                    var workoutDto = workout.ToDetailedResponse();

                    //var exercisesDto = workout.Exercises.Select(x => x.ToResponse()).ToList();

                    response = new DashboardResponse(
                        user,
                        workout,
                        upcomingWorkout.Start,
                        workoutDto.Exercises
                    );
                }


                return Ok(response);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
