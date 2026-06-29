using OnlineBookStore.Models;

namespace OnlineBookStore.Interfaces
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetBookById(int id);
        void AddBook(Book book);
    }
}
