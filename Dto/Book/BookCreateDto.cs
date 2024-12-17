using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Book;

public class BookCreateDto
{
    [StringLength(100)]
    public required string Author { get; set; }
    [StringLength(100)]
    public required string Title { get; set; }
    [StringLength(400)]
    public required string Description { get; set; }
    public int Released { get; set; }
}
