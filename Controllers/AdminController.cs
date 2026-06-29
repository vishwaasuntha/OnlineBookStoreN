using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;

namespace OnlineBookStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DASHBOARD
        public IActionResult Dashboard()
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.TotalBooks = _context.Books.Count();
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.TotalCustomers = _context.Users.Count(x => x.Role == "Customer");

            return View();
        }

        // VIEW CUSTOMERS
        public IActionResult Customers()
        {
            var customers = _context.Users
                .Where(u => u.Role == "Customer")
                .ToList();

            return View(customers);
        }

        // VIEW ALL ORDERS
        public IActionResult Orders()
        {
            string role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = _context.Orders.ToList();
            return View(orders);
        }
        //add order management
        public IActionResult ManageOrders()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                .Include(o => o.User)
                .ToList();
            return View(orders);
        }

        // UPDATE ORDER STATUS
        public IActionResult UpdateStatus(int id, string status)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");


            var order = _context.Orders.Find(id);

            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
            }

            return RedirectToAction("ManageOrders");
        }
    }
}