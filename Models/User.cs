using System;

namespace backend.Models;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}