using OnlineBookStore.Models;
using System.Text.Json;

namespace OnlineBookStore.Services
{
    public class ExportService
    {
        public string ExportBooksToJson(List<Book> books)
        {
            return JsonSerializer.Serialize(books);
        }
    }
}
