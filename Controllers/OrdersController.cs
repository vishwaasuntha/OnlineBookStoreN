using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;

namespace OnlineBookStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ORDER HISTORY
        public IActionResult MyOrders()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            var orders = _context.Orders
                .Where(o => o.UserId == userId.Value)
                .ToList();

            return View(orders);
        }

        // ORDER DETAILS
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefault(o => o.Id == id);

            return View(order);
        }
    }
}