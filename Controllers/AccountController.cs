using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, string ConfirmPassword)
        {
            if (string.IsNullOrWhiteSpace(user.Email) ||
        string.IsNullOrWhiteSpace(user.PasswordHash) ||
        string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ViewBag.Error = "All fields are required";
                return View();
            }

            if (user.PasswordHash != ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View();
            }

            if (_context.Users.Any(x => x.Email == user.Email))
            {
                ViewBag.Error = "Email already exists";
                return View();
            }

            try
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                user.Role = "Customer";

                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.InnerException?.Message ?? ex.Message;
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // 🔥 HARD CODED ADMIN LOGIN
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required";
                return View();
            }

            if (email == "admin@bookstore.com" && password == "admin123")
            {
                HttpContext.Session.SetInt32("UserId", 0);
                HttpContext.Session.SetString("UserName", "Admin");
                HttpContext.Session.SetString("Role", "Admin");
                return RedirectToAction("Dashboard", "Admin");
            }

            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                ViewBag.Error = "Invalid email";
                return View();
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isValid)
            {
                ViewBag.Error = "Invalid password";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}