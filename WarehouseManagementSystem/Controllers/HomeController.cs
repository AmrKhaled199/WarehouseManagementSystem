using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WarehouseManagementSystem.Filters;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    [AuthFilter]

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.userRole = HttpContext.Session.GetString("UserRole");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult Dashboard()
        {
            return View();
        }

    }
}
