using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Dto.CalendarEvents;
using FitnessTrackerBackend.Dto.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // => /api/users
    public class CalendarController : Controller
    {
        private readonly AppDbContext _context;
        public CalendarController(AppDbContext context) => _context = context;


        // GET: Calendar events
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CalendarEventResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCalendarEvents(CancellationToken ct)
        {
            try
            {
                var events  = await _context.Calendar
                    .OrderBy(e => e.Start)
                    .Select(e => new CalendarEventResponse(
                        e.Id,
                        e.WorkoutPlanId,
                        e.Title,
                        e.Start,
                        e.End,
                        e.AllDay
                    ))
                    .ToListAsync(ct);

                return Ok(events);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred when trying to retrieve all calendar events. Exception: {ex.Message}");
                return StatusCode(500, "An error occured when trying to retrieve all calendar events.");
            }
        }

        // GET: Calendar event by workoutplan ID
        [HttpGet("by-workoutplan/{id}")]
        [ProducesResponseType(typeof(CalendarEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CalendarEventResponse>>> GetCalendarEvents(Guid id, CancellationToken ct) 
        {
            try
            {
                var calendarEvents = await _context.Calendar
                    .Where(x => x.WorkoutPlanId == id)
                    .OrderBy(x => x.Start)
                    .Select(x => new CalendarEventResponse(
                        x.Id,
                        x.WorkoutPlanId,
                        x.Title,
                        x.Start,
                        x.End,
                        x.AllDay
                    ))
                    .ToListAsync();

                return Ok(calendarEvents);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred when trying to retrieve calendar events by workoutplan ID. Exception: {ex.Message}");
                return StatusCode(500, "An error occured when trying to retrieve calendar events by workoutplan ID.");
            }
        }

        // POST: Create a new Calendar event
        [HttpPost]
        [ProducesResponseType(typeof(CalendarEventResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCalendarEvent(CreateCalendarEventRequest request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                ModelState.AddModelError(nameof(request.Title), "Title is required.");
            }
            if(request.End <= request.Start)
            {
                ModelState.AddModelError(nameof(request.End), "End time must be after start time.");
            }
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var entity = request.ToModel();
                _context.Calendar.Add(entity);
                await _context.SaveChangesAsync(ct);

                var response = entity.ToResponse();
                return CreatedAtAction(nameof(GetCalendarEvents), new { id = response.Id }, response);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred when trying to create a calendar event. Exception: {ex.Message}");
                return StatusCode(500, "An error occured when trying to create a new calendar event.");
            }
        }

        // DELETE: Calendar event by ID
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCalendarEvent(Guid id, CancellationToken ct)
        {
            try
            {
                var calendarEvent = await _context.Calendar.FindAsync(id, ct);
                if (calendarEvent == null)
                {
                    return NotFound();
                }
                _context.Calendar.Remove(calendarEvent);
                _context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred when trying to delete a calendar event. Exception: {ex.Message}");
                return StatusCode(500, "An error occured when trying to delete the calendar event.");
            }
        }
    }
}
