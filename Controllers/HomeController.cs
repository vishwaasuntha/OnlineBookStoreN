using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models.ViewModels;

namespace OnlineBookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search)
        {
            var books = _context.Books
            .Include(b => b.Category)
            .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(b =>
                    b.Title.Contains(search) ||
                    b.Author.Contains(search));
            }

            var model = new HomeViewModel
            {
                Categories = _context.Categories.ToList(),
                Books = books.ToList()
            };

            return View(model);

            /*   var model = new HomeViewModel
               {
                   Categories = _context.Categories.ToList(),
                   Books = _context.Books.ToList()
               };


               var books = _context.Books
                   .Include(b => b.Category)
                   .AsQueryable();

               if (!string.IsNullOrEmpty(search))
               {
                   books = books.Where(b =>
                       b.Title.Contains(search) ||
                       b.Author.Contains(search));
               }

               return View(books.ToList());*/
        }

        public IActionResult BooksByCategory(int id)
        {
            var books = _context.Books
                .Where(b => b.CategoryId == id)
                .ToList();

            return View(books);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}