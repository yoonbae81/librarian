using System;

namespace Librarian.Entities;
public class Book(string title, string author)
{
    public string? Title { get; }
    public string? Author { get; }
}
