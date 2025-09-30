using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Dto.Dashboard;
using FitnessTrackerBackend.Dto.Mappings;
using FitnessTrackerBackend.Models;
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
                // Find user by user id
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                // Find the workoutplan by user id that has the nearest calendar event date
                var workout = await _context.WorkoutPlans
                    .Include(wp => wp.CalendarEvents)
                    .Include(wp => wp.Exercises)
                        .ThenInclude(x => x.Progressions.Where(u => u.UserId == id))
                    .Where(wp => wp.UserId == id && wp.CalendarEvents.Count > 0)
                        .OrderBy(wp => wp.CalendarEvents
                            .Where(c => c.Start >= DateTime.Now)
                            .Select(c => c.Start)
                            .Min())
                    .FirstOrDefaultAsync(ct);

                // If no workoutplan with calendar events found, get any workoutplan for the user, so other metrics can also be displayed
                if (workout == null)
                {
                    workout = await _context.WorkoutPlans
                        .Include(wp => wp.CalendarEvents)
                        .Include(wp => wp.Exercises)
                            .ThenInclude(x => x.Progressions.Where(u => u.UserId == id))
                        .Where(wp => wp.UserId == id)
                        .FirstOrDefaultAsync(ct);

                    if(workout == null)
                    {
                        return NotFound();
                    }
                }

                CalendarEvent upcomingWorkout = null;

                if (workout.CalendarEvents.Count > 0)
                {
                    // Get the first calendar event based on date.
                    upcomingWorkout = workout.CalendarEvents?.OrderBy(x => x.Start).FirstOrDefault();
                }

                var workoutDto = workout.ToDetailedResponse();

                var response = new DashboardResponse(
                    user,
                    workout,
                    upcomingWorkout?.Start,
                    workoutDto.Exercises
                );

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
