using System.Collections.Generic;

namespace OnlineBookStore.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Book> Books { get; set; }
    }
}
