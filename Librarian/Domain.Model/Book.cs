using System;

namespace Librarian.Domain.Model;
public class Book
{
    public required string Title { init; get; }
    public required string Author { init; get; }

    public string? category { get; set; }
}
