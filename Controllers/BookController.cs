using System.Diagnostics;
using backend.Data;
using backend.Dto.Book;
using backend.Mapper;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO: Implement pagination for large book collections
        /// <summary>
        /// Gets a list of all books
        /// </summary>
        /// <returns>List of books</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        /// <summary>
        /// Gets a book by id from the database
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <returns>Book by id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        /// <summary>
        /// Creates a new book entry in the database
        /// </summary>
        /// <param name="bookDto">Data transfer object containing book details</param>
        /// <returns>Created book with assigned ID</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Book>> CreateBook([FromBody] BookCreateDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = BookMapper.ToBook(bookDto);

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        /// <summary>
        /// Deletes a book entry in the database by id
        /// </summary>
        /// <param name="id">Book Id</param>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Updates a book in the database based on id
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <param name="updatedBook">Updated book info</param>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            if (id != updatedBook.Id)
            {
                return BadRequest("Book ID mismatch");
            }

            var existingBook = await _context.Books.FindAsync(id);

            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Author = updatedBook.Author;
            existingBook.Title = updatedBook.Title;
            existingBook.Description = updatedBook.Description;
            existingBook.Released = updatedBook.Released;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
