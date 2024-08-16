using System;

namespace Librarian.Domain.Model;
public class Book
{
    public required string Title { init; get; }
    public required string Author { init; get; }
    public required string Publisher { init; get; }
    public required string PublishDate { init; get; },
    public required string Link { init; get; }
    public string? Category { get; set; }
}
