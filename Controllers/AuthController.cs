using backend.Data;
using backend.Dto;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Service;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        public AuthController(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return NotFound("Invalid password or email");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return NotFound("Invalid password or email");
            }

            var token = _tokenService.GenerateToken(user);

            var response = new AuthResponseDto()
            {
                Email = user.Email,
                Token = token
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return Conflict("User already exists with this email");
            }

            var user = new User()
            {
                Email = request.Email!,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)!
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _tokenService.GenerateToken(user);

            var response = new AuthResponseDto()
            {
                Email = user.Email,
                Token = token
            };

            return Ok(response);
        }
    }
}
