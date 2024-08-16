using Librarian.Domain.Model;
using Librarian.Port.Out;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarian.Adapter.Out;
public class KoreanNationalLibraryAdapter : ISearchBookPort
{
    private const string KEY_NAME = "NLGOKR_API_KEY";

    private readonly string _key;
    private readonly string _baseUrl = "https://www.nl.go.kr/NL/search/openApi/search.do?key={}&category=도서&srchTarget=title&kwd={}";

    public KoreanNationalLibraryAdapter()
    {
        _key = Environment.GetEnvironmentVariable("KEY")
               ?? throw new InvalidOperationException("NLGOKR_API_KEY is not set in the environment variables.");
    }

    public async Task<Book> Search(string keyword)
    {
        // Construct the URL with the API key and keyword
        string url = string.Format(CultureInfo.InvariantCulture, _baseUrl, _key, Uri.EscapeDataString(keyword));

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request to Korean National Library failed with status code {response.StatusCode}");
            }

            string xmlContent = await response.Content.ReadAsStringAsync();

            // Parse the XML content
            XDocument xmlDoc = XDocument.Parse(xmlContent);

            // Extract book information - this example assumes certain XML structure
            var item = xmlDoc.Descendants("item").FirstOrDefault();
            if (item == null)
            {
                throw new InvalidOperationException("No books found with the given keyword.");
            }

            Book book = new Book
            {
                Title = item.Element("title_info").Value,
                Author = item.Element("author_info").Value,
                Publisher = item.Element("pub_info")?.Value,
                PublishDate = item.Element("pub_year_info")?.Value,
                Link = item.Element("detail_link")?.Value,
                Category = item.Element("class_no")?.Value,
                // You can add more properties as needed
            };

            return book;
        }
    }
}
