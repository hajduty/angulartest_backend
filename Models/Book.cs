using System;

namespace backend.Models;

public class Book
{
    public int Id { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Released { get; set; }
}