using Librarian.Application.Ports.Output;
using Librarian.Domain.Entities;
using Librarian.Domain.Services;

namespace Librarian.Infrastructure.Adapters;
public class LocalLibraryAdapter : ILocalLibraryPort
{
    private string _baseDir;
    private Dictionary<string, string> _classificationCodes;

    public LocalLibraryAdapter(string baseDir, string classificationCodeFilePath)
    {
        _baseDir = baseDir;
        _classificationCodes = ReadClassificationCodeFile(classificationCodeFilePath);
    }

    public async Task<string> Organize(Book book)
    {
        var code = BookService.GetClassificationCode(book.Code, _classificationCodes);

        var targetDirectory = Path.Combine(_baseDir, $"{code.Code} {code.Name}");
        Directory.CreateDirectory(targetDirectory);

        var movedPath = Path.Combine(targetDirectory, Path.GetFileName(book.FilePath));
        File.Move(book.FilePath, movedPath);

        return movedPath;
    }

    private Dictionary<string, string> ReadClassificationCodeFile(string filePath)
    {
        var dic = new Dictionary<string, string>();

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Length < 4) continue; // Skip empty or invalid lines
            var key = line.Substring(0, 3);
            var value = line.Substring(4).Trim();
            dic[key] = value;
        }

        return dic;
    }
}
