namespace Librarian.Application.Ports.Input;
public interface IOrganizeBooksUseCase
{
    Task Execute(IEnumerable<string> bookFiles);
}
