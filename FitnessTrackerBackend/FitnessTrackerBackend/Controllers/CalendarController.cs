using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Models;
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
            try
            {
                var calendarEvents = _context.Calendar.ToList();
                return Ok(calendarEvents);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: Create a new Calendar event
        [HttpPost]
        public IActionResult CreateCalendarEvent(CalendarEvent calendarEvent)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    calendarEvent.Id = Guid.NewGuid();
                    _context.Calendar.Add(calendarEvent);
                    _context.SaveChanges();
                    return CreatedAtAction(nameof(GetAllCalendarEvents), new { id = calendarEvent.Id }, calendarEvent);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                //return StatusCode(500, "Internal server error");
            }
         
            return BadRequest(ModelState);
        }

        // DELETE: Calendar event by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteCalendarEvent(Guid id)
        {
            try
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
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                //return StatusCode(500, "Internal server error");
                return Ok();
            }
        }
    }
}
