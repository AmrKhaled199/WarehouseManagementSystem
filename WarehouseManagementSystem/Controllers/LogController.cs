using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Filters;
using WMS.DAL;

namespace WarehouseManagementSystem.Controllers
{
    [AuthFilter]
    public class LogController : Controller
    {
        private readonly AppDbContext _context;

        public LogController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var logs = _context.Notifications
                .OrderByDescending(n => n.SentAt)
                .ToList();

            return View(logs);
        }
    }
}
