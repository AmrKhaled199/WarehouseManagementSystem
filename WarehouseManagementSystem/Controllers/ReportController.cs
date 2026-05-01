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

        public IActionResult DownloadProducts(int? month, int? year)
        {
            var file = _reportService.GenerateProductsReport(month, year);
            string fileName = month.HasValue ? $"Products_{month}_{year}.xlsx" : "Products_All.xlsx";
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public IActionResult DownloadStorageFees(int? month, int? year)
        {
            var file = _reportService.GenerateStorageFeesReport(month, year);
            string fileName = month.HasValue ? $"StorageFees_{month}_{year}.xlsx" : "StorageFees_All.xlsx";
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public IActionResult DownloadLogs(int? month, int? year)
        {
            var file = _reportService.GenerateLogsReport(month, year);
            string fileName = month.HasValue ? $"Logs_{month}_{year}.xlsx" : "Logs_All.xlsx";
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}