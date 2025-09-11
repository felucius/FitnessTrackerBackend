using FitnessTrackerBackend.Data;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAllCalendarEvents()
        {
            var calendarEvents = _context.Calendar.ToList();
            return Ok(calendarEvents);
        }

        // DELETE: Calendar event by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteCalendarEvent(Guid id)
        {
            var calendarEvent = _context.Calendar.Find(id);
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
