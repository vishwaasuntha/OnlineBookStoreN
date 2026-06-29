using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class CategoriesController : Controller
    {
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // CREATE
        public IActionResult Create()
        {
            if (!IsAdmin())
            return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var category = _context.Categories.Find(id);

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            _context.Categories.Update(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}