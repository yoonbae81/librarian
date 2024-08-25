using Librarian.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarian.Application.Ports.Output;
public interface IOnlineLibraryPort
{
    public Task<Book> Search(string keyword);
}
