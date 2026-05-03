using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Filters;
using WMS.BLL.Interfaces;

namespace WarehouseManagementSystem.Controllers
{
    [AdminOnlyFilter]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetAll();
            return View(users);
        }


        [HttpPost]
        
        public IActionResult ChangeRole (int id , string newRole)
        {
            // منع الـ Admin يغير Role نفسه
            var currentUserId = HttpContext.Session.GetString("UserId");
            if (currentUserId == id.ToString())
            {
                TempData["Error"] = "You cannot change your own role.";
                return RedirectToAction("Index");
            }

            bool result = _userService.ChangeRole(id, newRole);

            if (!result)
                TempData["Error"] = "Failed to change role.";
            else
                TempData["Success"] = "Role updated successfully!";

            return RedirectToAction("Index");
        }
    }
}
