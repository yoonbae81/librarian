using System.Xml.Linq;
using System.Globalization;
using Librarian.Domain.Entities;
using Librarian.Application.Ports.Output;
using Librarian.Domain.Services;

namespace Librarian.Infrastructure.Adapters;
public class KoreanNationalLibraryAdapter(string apiKey) : IOnlineLibraryPort
{
    private const string _baseUrl = "https://www.nl.go.kr";
    private readonly string _searchUrl = "/NL/search/openApi/search.do?key={0}&category=도서&kwd={1}";

    public async Task<Book> Search(string filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var title = BookService.ParseTitle(fileName);
        var author = BookService.ParseAuthor(fileName);

        string url = string.Format(
            CultureInfo.InvariantCulture,
            _searchUrl,
            apiKey,
            Uri.EscapeDataString($"{title.Replace(" ", "")}"));

        string xmlResult = await MakeHttpRequest(_baseUrl + url);
        Book book = ParseValues(xmlResult, filePath);

        return book;
    }

    private async Task<string> MakeHttpRequest(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request to Korean National Library failed with status code {response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }

    private Book ParseValues(string xmlContent, string filePath)
    {
        XDocument xmlDoc = XDocument.Parse(xmlContent);

        var item = xmlDoc.Descendants("item").FirstOrDefault()
            ?? throw new InvalidOperationException("No search results found.");

        while (string.IsNullOrEmpty(item.Element("class_no")?.Value))
        {
            item = item.ElementsAfterSelf("item").FirstOrDefault()
                ?? throw new InvalidOperationException("No more search results found.");
        }

        Book book = new Book
        {
            FilePath = filePath,
            Title = item.Element("title_info").Value,
            Author = item.Element("author_info").Value,
            Code = item.Element("class_no").Value,
        };

        return book;
    }
}
