using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Dto.CalendarEvents;
using FitnessTrackerBackend.Dto.Mappings;
using FitnessTrackerBackend.Models;
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

        // GET: Calendar event by workoutplan ID
        [HttpGet("by-workoutplan/{id}")]
        [ProducesResponseType(typeof(CalendarEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CalendarEventResponse>>> GetCalendarEvents(Guid id, CancellationToken ct) 
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

            var entity = request.ToModel();
            _context.Calendar.Add(entity);
            await _context.SaveChangesAsync(ct);

            var response = entity.ToResponse();
            // Need to test this result
            return CreatedAtAction(nameof(GetCalendarEvents), new { id = response.Id }, response);
        }

        // DELETE: Calendar event by ID
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCalendarEvent(Guid id, CancellationToken ct)
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
    }
}
