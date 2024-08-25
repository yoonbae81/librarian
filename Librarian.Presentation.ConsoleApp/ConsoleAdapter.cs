using Librarian.Application.Ports.Input;
using Librarian.Application.Ports.Output;
using Librarian.Domain.Entities;
using Librarian.Domain.Services;

namespace Librarian.ConsoleApp;
internal class ConsoleAdapter : IOrganizeBooksUseCase
{
    private readonly IOnlineLibraryPort onlineLibrary;
    private readonly ILocalLibraryPort localLibrary;

    public ConsoleAdapter(IOnlineLibraryPort onlineLibrary, ILocalLibraryPort localLibrary)
    {
        this.onlineLibrary = onlineLibrary;
        this.localLibrary = localLibrary;
    }

    public async Task Execute(IEnumerable<string> bookFiles)
    {
        Console.WriteLine($"Organizing {bookFiles.Count()} books in {Directory.GetCurrentDirectory()}");

        foreach (var filePath in bookFiles)
        {
            Console.WriteLine($"[Search] {Path.GetFileName(filePath)}");

            try
            {
                var book = await onlineLibrary.Search(filePath);
                Console.WriteLine($"[Result] {book}");

                var parsedTitle = BookService.ParseTitle(Path.GetFileNameWithoutExtension(filePath));
                var parsedAuthor = BookService.ParseAuthor(Path.GetFileNameWithoutExtension(filePath));
                if (!ContainsParts(book.Title.Replace(" ", ""), parsedTitle.Replace(" ", ""), 10)
                    && !ContainsParts(book.Author.Replace(" ", ""), parsedAuthor.Replace(" ", ""), 3))
                {
                    Console.Beep(300, 500);
                    Console.WriteLine("Press Enter if the information is correct to move the file");
                    if (Console.ReadKey().Key != ConsoleKey.Enter)
                    {
                        continue;
                    }
                }

                var movedPath = await localLibrary.Organize(book);
                Console.WriteLine($"[Moved] {Path.GetDirectoryName(movedPath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }
            Console.WriteLine();
        }

        Console.WriteLine("Books organized successfully.");
    }

    private static bool ContainsParts(string str1, string str2, int atLeast)
    {
        for (int i = 0; i <= str1.Length - atLeast; i++)
        {
            for (int j = 0; j <= str2.Length - atLeast; j++)
            {
                if (str1.Substring(i, atLeast) == str2.Substring(j, atLeast))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
