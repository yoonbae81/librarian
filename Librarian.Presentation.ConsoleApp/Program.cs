using Librarian.Application.Ports.Input;
using Librarian.Application.Ports.Output;
using Librarian.ConsoleApp;
using Librarian.Infrastructure.Adapters;

const string KEY_NAME = "API_KEY_LIBRARY";
var apiKey = Environment.GetEnvironmentVariable(KEY_NAME)
               ?? throw new InvalidOperationException($"{KEY_NAME} is not set in the environment variables.");
IOnlineLibraryPort onlineLibrary = new KoreanNationalLibraryAdapter(apiKey);

string baseDir = Directory.GetCurrentDirectory();
string codeFile = Path.Combine(Directory.GetCurrentDirectory(), "KDC.txt");

ILocalLibraryPort localLibrary 
    = new LocalLibraryAdapter(baseDir, codeFile);

IOrganizeBooksUseCase organizeBooksUseCase
    = new ConsoleAdapter(onlineLibrary, localLibrary);

var books = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.epub")
    .Concat(Directory.GetFiles(Directory.GetCurrentDirectory(), "*.pdf"))
    .ToList();

await organizeBooksUseCase.Execute(books);


// Move all epub and pdf files to NotFound directory
/*
var notFoundDir = Path.Combine(Directory.GetCurrentDirectory(), "NotFound");
Directory.CreateDirectory(notFoundDir);

var notFoundFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.epub")
    .Concat(Directory.GetFiles(Directory.GetCurrentDirectory(), "*.pdf"))
    .ToList();

foreach (var file in notFoundFiles)
{
    var movedPath = Path.Combine(notFoundDir, Path.GetFileName(file));
    File.Move(file, movedPath);
    Console.WriteLine($"[NotFound] {Path.GetFileName(file)}");
}
*/