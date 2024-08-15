using Librarian.Domain.Model;
using Librarian.Port.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarian.Adapter.Out;
public class KoreanNationalLibraryAdapter : SearchBookPort
{
    private readonly string baseUrl = "https://www.nl.go.kr/NL/search/openApi/search.do?key={}&category=도서&srchTarget=title&kwd={}";
    public Book Search(string keyword)
    {
        // TODO http request to nl.go.kr and parse xml 
        throw new NotImplementedException();
    }
}
