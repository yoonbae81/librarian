using Librarian.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarian.Port.Out;
internal interface SearchBookPort
{
    public Book Search(string keyword);
}
