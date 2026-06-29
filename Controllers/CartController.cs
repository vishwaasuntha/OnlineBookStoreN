using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // VIEW CART
        public IActionResult Index()
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

            var cartItems = _context.CartItems
             .Include(c => c.Book)   // ⭐ IMPORTANT FIX
             .Where(c => c.UserId == userId.Value)
             .ToList();

            return View(cartItems);
        }

        // ADD TO CART
        public IActionResult AddToCart(int bookId)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            int userId = (int)HttpContext.Session.GetInt32("UserId");

            var item = _context.CartItems
                .FirstOrDefault(c => c.BookId == bookId && c.UserId == userId);

            if (item == null)
            {
                _context.CartItems.Add(new CartItem
                {
                    BookId = bookId,
                    UserId = userId,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // UPDATE QUANTITY
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var item = _context.CartItems.Find(id);

            if (item != null)
            {
                item.Quantity = quantity;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // REMOVE ITEM
        public IActionResult Remove(int id)
        {
            var item = _context.CartItems.Find(id);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // CHECKOUT
        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            int userId = (int)HttpContext.Session.GetInt32("UserId");

            var cartItems = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cartItems.Any())
                return RedirectToAction("Index");

            decimal total = cartItems.Sum(x => x.Book.Price * x.Quantity);

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Book.Price
                });
            }

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            return RedirectToAction("MyOrders", "Orders");
        }
    }
}