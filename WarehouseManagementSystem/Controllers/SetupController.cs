using Microsoft.AspNetCore.Mvc;
using WMS.BLL.Interfaces;
using WMS.DAL;
using WMS.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class SetupController : Controller
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;

        public SetupController(IAuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // لو فيه Admin خلاص — ارجع للـ Login
        private bool AdminExists() => _context.Users.Any(u => u.Role == "Admin");

        public IActionResult Index()
        {
            if (AdminExists()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public IActionResult Index(string fullName, string email, string password, string confirmPassword)
        {
            if (AdminExists()) return RedirectToAction("Login", "Account");

            // Validation
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            if (password.Length < 6)
            {
                ViewBag.Error = "Password must be at least 6 characters.";
                return View();
            }

            if (_authService.IsEmailExists(email))
            {
                ViewBag.Error = "Email already in use.";
                return View();
            }

            var admin = new User
            {
                FullName = fullName,
                Email = email,
                Role = "Admin"
            };

            _authService.Register(admin, password);

            // Auto-login
            var newAdmin = _authService.Login(email, password);
            HttpContext.Session.SetString("UserId", newAdmin.Id.ToString());
            HttpContext.Session.SetString("UserName", newAdmin.FullName);
            HttpContext.Session.SetString("UserRole", newAdmin.Role);

            return RedirectToAction("Index", "Home");
        }
    }
}
