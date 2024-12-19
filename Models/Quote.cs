using System;

namespace backend.Models;

public class Quote
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public int UserId { get; set; }
    public required User User { get; set; }
}