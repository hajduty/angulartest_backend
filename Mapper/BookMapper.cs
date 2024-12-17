using System;
using backend.Dto.Book;
using backend.Models;

namespace backend.Mapper;

public static class BookMapper
{
    public static Book ToBook(BookCreateDto dto)
    {
        return new Book
        {
            Author = dto.Author,
            Title = dto.Title,
            Description = dto.Description,
            Released = dto.Released
        };
    }
}
