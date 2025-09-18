using FitnessTrackerBackend.Data;
using FitnessTrackerBackend.Dto.Mappings;
using FitnessTrackerBackend.Dto.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // => /api/users
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        
        // GET /api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll(CancellationToken ct)
        {
            try
            {
                var users = await _context.Users
                    .AsNoTracking()
                    .ToListAsync(ct);

                var userDto = users.Select(u => u.ToResponse()).ToList();

                return Ok(userDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET /api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(Guid id, CancellationToken ct)
        {
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id, ct);

                if (user == null)
                {
                    return NotFound();
                }
                
                return Ok(user.ToResponse());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("by-email")]
        public async Task<ActionResult<UserResponse>> GetByEmail([FromQuery] string email, CancellationToken ct)
        {
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == email, ct);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user.ToResponse());
            }
            catch(Exception ex) {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST /api/users
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create(CreateUserRequest user, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                ModelState.AddModelError(nameof(user.Name), "Name is required.");
            }
            
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                ModelState.AddModelError(nameof(user.Email), "Email is required.");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var userDto = user.ToModel();
                _context.Users.Add(userDto);
                await _context.SaveChangesAsync(ct);

                var response = userDto.ToResponse();
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT /api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> Update(Guid id, CreateUserRequest user, CancellationToken ct)
        {
            try
            {
                var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

                if (entity == null)
                {
                    return NotFound();
                }
                
                entity.Name = user.Name;
                entity.Gender = user.Gender;
                entity.Email = user.Email;
                entity.Age = user.Age;
                entity.Weight = user.Weight;
                entity.Height = user.Height;

                await _context.SaveChangesAsync(ct);
                return Ok(entity.ToResponse());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
