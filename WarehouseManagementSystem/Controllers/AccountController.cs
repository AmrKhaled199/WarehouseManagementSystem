using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.ViewModels;
using WMS.BLL.Interfaces;
using WMS.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _authService.Login(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role);

            return RedirectToAction("Index", "Home");
        }
        


        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Role = model.Role
            };

            bool result = _authService.Register(user, model.Password);

            if (!result)
            {
                ModelState.AddModelError("", "Registration failed. Email might already be in use.");
                return View(model);
            }
            var newUser = _authService.Login(model.Email, model.Password);
            HttpContext.Session.SetString("UserId", newUser.Id.ToString());
            HttpContext.Session.SetString("UserName", newUser.FullName);
            HttpContext.Session.SetString("UserRole", newUser.Role);

            return RedirectToAction("Index", "Home");
            
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
