using Librarian.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarian.Ports.Output;
internal interface LibraryPort
{
    public string SearchCallNumber(Book book);
}
