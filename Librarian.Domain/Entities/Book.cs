using Domain.ValueObjects;
using System;

namespace Librarian.Domain.Entities;
public class Book
{
    public required string FilePath { init; get; }
    public required string Title { init; get; }
    public required string Author { init; get; }
    public required string Code { init; get; }

    public override string ToString()
    {
        return $"{Title} - {Author} [{Code}]";
    }
}
