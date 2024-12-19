using System.Security.Claims;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO: Implement pagination for large quote collections
        /// <summary>
        /// Gets a list of all quotes for a user
        /// </summary>
        /// <returns>List of quotes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            var userQuotes = await _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Quotes)
                .ToListAsync();

            return userQuotes;
        }

        /// <summary>
        /// Gets a quote by id from the database
        /// </summary>
        /// <param name="id">Quote Id</param>
        /// <returns>Quote by id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            if (quote == null || quote.UserId != userId)
            {
                return NotFound();
            }

            return quote;
        }

        /// <summary>
        /// Creates a new quote entry in the database for the user
        /// </summary>
        /// <param name="text">Quote details</param>
        /// <returns>Created quote with assigned ID</returns>
        [HttpPost]
        public async Task<ActionResult<Quote>> CreateQuote([FromBody] string text)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            var quote = new Quote
            {
                Text = text,
                UserId = userId,
            };

            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuote), new { id = quote.Id }, quote);
        }

        /// <summary>
        /// Deletes a quote entry in the database by id
        /// </summary>
        /// <param name="id">Quote Id</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);

            if (quote == null)
            {
                return NotFound();
            }

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
