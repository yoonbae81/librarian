using Domain.ValueObjects;
using Librarian.Domain.Entities;

namespace Librarian.Application.Ports.Output;
public interface ILocalLibraryPort
{
    Task<string> Organize(Book book);
}
