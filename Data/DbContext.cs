using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public required DbSet<User> Users { get; set; }
    public required DbSet<Book> Books { get; set; }
    public required DbSet<Quote> Quotes { get; set; }
}
