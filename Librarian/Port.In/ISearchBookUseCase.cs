using Librarian.Domain.Model;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Librarian.Port.In;
internal interface ISearchBookUseCase
{
    public Book Search(string keyword);
}
