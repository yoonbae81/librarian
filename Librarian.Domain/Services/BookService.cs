using Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace Librarian.Domain.Services;
public class BookService
{
    public static string ParseTitle(string fileName, string separator = "-")
    {
        ArgumentNullException.ThrowIfNull(fileName);
        ArgumentNullException.ThrowIfNull(separator);

        string keyword;
        int lastDashIndex = fileName.LastIndexOf(separator);
        if (lastDashIndex != -1)
        {
            keyword = fileName[..lastDashIndex].Trim();
        }
        else
        {
            keyword = fileName.Trim();
        }

        // Remove text inside parentheses
        int openParenIndex = keyword.LastIndexOf('(');
        while (openParenIndex != -1)
        {
            int closeParenIndex = keyword.LastIndexOf(')');
            if (closeParenIndex != -1 && closeParenIndex > openParenIndex)
            {
                keyword = keyword.Remove(openParenIndex, closeParenIndex - openParenIndex + 1).Trim();
            }
            else
            {
                break;
            }
            openParenIndex = keyword.LastIndexOf('(');
        }

        return keyword;
    }

    public static string ParseAuthor(string fileName, string separator = "-")
    {
        ArgumentNullException.ThrowIfNull(fileName);
        ArgumentNullException.ThrowIfNull(separator);

        string author;
        int lastDashIndex = fileName.LastIndexOf(separator);
        if (lastDashIndex != -1)
        {
            author = fileName[(lastDashIndex + 1)..].Trim();
        }
        else
        {
            author = string.Empty;
        }

        return author;
    }

    public static ClassificationCode GetClassificationCode(string code, Dictionary<string, string> availableCodesDict)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length < 3 || !Regex.IsMatch(code.AsSpan(0, 3), @"^\d{3}$"))
        {
            throw new ArgumentException("The first three characters of the Classification code must be a valid three-digit number.");
        }

        var mostSimilarCode = GetMostSimilarCode(code, availableCodesDict.Keys) 
            ?? throw new ArgumentException("No similar code found in the dictionary.");

        return new ClassificationCode
        {
            Code = mostSimilarCode,
            Name = availableCodesDict[key: mostSimilarCode]
        };
    }
    private static string GetMostSimilarCode(string code, IEnumerable<string> availableCodes)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length < 3 || !Regex.IsMatch(code.AsSpan(0, 3), @"^\d{3}$"))
        {
            throw new ArgumentException("The first three characters of the Classification code must be a valid three-digit number.");
        }

        var codeSpan = code[..3];
        var mostSimilarCode = availableCodes
            .Select(c => new { Code = c, Similarity = GetSimilarity(codeSpan, c.AsSpan(0, 3)) })
            .OrderByDescending(c => c.Similarity)
            .First().Code;

        return mostSimilarCode;
    }

    private static int GetSimilarity(ReadOnlySpan<char> code1, ReadOnlySpan<char> code2)
    {
        var similarity = 0;
        for (var i = 0; i < 3; i++)
        {
            if (code1[i] == code2[i])
            {
                similarity++;
            }
        }

        return similarity;
    }
}
