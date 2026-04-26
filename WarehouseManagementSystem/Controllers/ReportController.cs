using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Filters;
using WMS.BLL.Interfaces;

namespace WarehouseManagementSystem.Controllers
{
    [AuthFilter]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DownloadProducts()
        {
            var file = _reportService.GenerateProductsReport();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
        }

        public IActionResult DownloadStorageFees()
        {
            var file = _reportService.GenerateStorageFeesReport();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StorageFees.xlsx");
        }

        public IActionResult DownloadLogs()
        {
            var file = _reportService.GenerateLogsReport();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Logs.xlsx");
        }
    }
}
