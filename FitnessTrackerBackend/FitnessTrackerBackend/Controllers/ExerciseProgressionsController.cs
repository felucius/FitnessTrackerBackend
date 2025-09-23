using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Dto.Mappings;
using FitnessTrackerBackend.Dto.Progressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // => /api/exerciseProgression
    public class ExerciseProgressionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExerciseProgressionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/exerciseProgressions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseProgressionResponse>>> GetAllProgression(CancellationToken ct)
        {
            try
            {
                var progressions = await _context.Progression
                    .AsNoTracking()
                    .ToListAsync(ct);

                var progressionDto = progressions.Select(p => p.ToResponse()).ToList();
                
                return Ok(progressionDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET /api/exerciseProgressions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseProgressionResponse>> GetProgressionById(Guid id, CancellationToken ct)
        {
            try
            {
                var progression = await _context.Progression
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id, ct);
                if (progression == null)
                {
                    return NotFound();
                }
                return Ok(progression.ToResponse());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET /api/exerciseProgressions/{id}
        [HttpGet("by-date/{date}")]
        public async Task<ActionResult<ExerciseProgressionResponse>> GetProgressionByDate(DateTime date, CancellationToken ct)
        {
            try
            {
                var progressionAddedDate = date.ToUniversalTime().AddHours(2);
                var progression = await _context.Progression.FirstOrDefaultAsync(x => x.Date == progressionAddedDate);

                if (progression == null)
                {
                    return NotFound();
                }
                return Ok(progression.ToResponse());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST /api/exerciseProgressions
        [HttpPost]
        public async Task<ActionResult<ExerciseProgressionResponse>> CreateProgression(CreateExerciseProgressionRequest request, CancellationToken ct)
        {
            if (request.UserId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(request.UserId), "UserId is required.");
            }
            
            if (string.IsNullOrWhiteSpace(request.ExerciseId))
            {
                ModelState.AddModelError(nameof(request.ExerciseId), "ExerciseId is required.");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var progressionDto = request.ToModel();
                _context.Progression.Add(progressionDto);
                await _context.SaveChangesAsync(ct);
                
                var response = progressionDto.ToResponse();
                return CreatedAtAction(nameof(GetProgressionById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT /api/exerciseProgressions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgressions(Guid id, CreateExerciseProgressionRequest exerciseProgression, CancellationToken ct)
        {
            try
            {
                var dateTime = exerciseProgression.Date.AddHours(2);
                var progression = await _context.Progression.FirstOrDefaultAsync(x => x.UniqueExerciseId == id && x.Date == dateTime);

                if (progression == null)
                {
                    return NotFound();
                }

                progression.Weight = exerciseProgression.Weight;
                progression.Reps = exerciseProgression.Reps;

                await _context.SaveChangesAsync(ct);
                return Ok(progression.ToResponse());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE /api/exerciseProgressions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgression(Guid id, CancellationToken ct)
        {
            try
            {
                var progression = await _context.Progression.FindAsync(id, ct);

                if (progression == null)
                {
                    return NotFound();
                }
                
                _context.Progression.Remove(progression);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
