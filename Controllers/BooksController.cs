using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace OnlineBookStore.Controllers
{
    public class BooksController : Controller
    {
        // actions here
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(
            string title,
            string author,
            int? categoryId)
        {
            var books = _context.Books
                .Include(b => b.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                books = books.Where(x =>
                    x.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(author))
            {
                books = books.Where(x =>
                    x.Author.Contains(author));
            }

            if (categoryId.HasValue)
            {
                books = books.Where(x =>
                    x.CategoryId == categoryId);
            }

            ViewBag.Categories =
                new SelectList(_context.Categories,
                               "Id",
                               "Name");
            ViewBag.IsAdmin = HttpContext.Session.GetString("Role") == "Admin";

            return View(books.ToList());
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");
                ViewBag.Categories = _context.Categories.ToList();
            /*  ViewBag.Categories =
                  new SelectList(_context.Categories,
                                 "Id",
                                 "Name");*/

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book, IFormFile BookImage)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            // Upload Image
            if (BookImage != null)
            {
                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(BookImage.FileName);

                string uploadFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "books");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await BookImage.CopyToAsync(stream);
                }

                book.ImageUrl = "/images/books/" + fileName;
            }

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {

            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var book = _context.Books.Find(id);

            if (book == null)
                return NotFound();

            /*   ViewBag.Categories =
                   new SelectList(_context.Categories,
                                  "Id",
                                  "Name",
                                  book.CategoryId);*/

            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            _context.Books.Update(book);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var book = _context.Books.Find(id);

            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}