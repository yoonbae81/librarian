using Librarian.Domain.Model;

namespace Tests.Unit;

[TestClass]
public class Entity
{
    [TestMethod]
    public void TestBook()
    {
        Book b = new()
        {
            Author = "유시민",
            Title = "문과남자의과학공부"
        };
    }
}