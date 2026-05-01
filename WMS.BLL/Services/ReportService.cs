using OfficeOpenXml;
using OfficeOpenXml.Style;
using WMS.BLL.Interfaces;
using WMS.DAL;

namespace WMS.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
            ExcelPackage.License.SetNonCommercialPersonal("YourName");
        }

        public byte[] GenerateProductsReport(int? month = null, int? year = null)
        {
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Products");

            // Header
            sheet.Cells[1, 1].Value = "ID";
            sheet.Cells[1, 2].Value = "Name";
            sheet.Cells[1, 3].Value = "Category";
            sheet.Cells[1, 4].Value = "Weight (kg)";
            sheet.Cells[1, 5].Value = "Status";
            sheet.Cells[1, 6].Value = "Sender";
            sheet.Cells[1, 7].Value = "Receiver";
            sheet.Cells[1, 8].Value = "Entry Date";
            sheet.Cells[1, 9].Value = "Exit Date";

            using (var range = sheet.Cells[1, 1, 1, 9])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkBlue);
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            // Filter by month/year
            var products = _context.Products.AsQueryable();
            if (month.HasValue && year.HasValue)
                products = products.Where(p => p.EntryDate.Month == month && p.EntryDate.Year == year);

            var list = products.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                sheet.Cells[i + 2, 1].Value = p.Id;
                sheet.Cells[i + 2, 2].Value = p.Name;
                sheet.Cells[i + 2, 3].Value = p.Category;
                sheet.Cells[i + 2, 4].Value = p.WeightKg;
                sheet.Cells[i + 2, 5].Value = p.Status;
                sheet.Cells[i + 2, 6].Value = p.SenderName;
                sheet.Cells[i + 2, 7].Value = p.ReceiverName;
                sheet.Cells[i + 2, 8].Value = p.EntryDate.ToString("dd/MM/yyyy");
                sheet.Cells[i + 2, 9].Value = p.ExitDate?.ToString("dd/MM/yyyy") ?? "Still in Warehouse";
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
        }

        public byte[] GenerateStorageFeesReport(int? month = null, int? year = null)
        {
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Storage Fees");

            sheet.Cells[1, 1].Value = "ID";
            sheet.Cells[1, 2].Value = "Product Name";
            sheet.Cells[1, 3].Value = "Weight (kg)";
            sheet.Cells[1, 4].Value = "Rate/Hour/Kg";
            sheet.Cells[1, 5].Value = "Total Fee ($)";
            sheet.Cells[1, 6].Value = "Calculated At";

            using (var range = sheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkGreen);
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            var fees = _context.StorageFees.AsQueryable();
            if (month.HasValue && year.HasValue)
                fees = fees.Where(f => f.CalculatedAt.Month == month && f.CalculatedAt.Year == year);

            var list = fees.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var f = list[i];
                var product = _context.Products.FirstOrDefault(p => p.Id == f.ProductId);
                sheet.Cells[i + 2, 1].Value = f.Id;
                sheet.Cells[i + 2, 2].Value = product?.Name ?? "Unknown";
                sheet.Cells[i + 2, 3].Value = product?.WeightKg ?? 0;
                sheet.Cells[i + 2, 4].Value = f.RatePerHourPerKg;
                sheet.Cells[i + 2, 5].Value = f.TotalFee;
                sheet.Cells[i + 2, 6].Value = f.CalculatedAt.ToString("dd/MM/yyyy");
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
        }

        public byte[] GenerateLogsReport(int? month = null, int? year = null)
        {
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Logs");

            sheet.Cells[1, 1].Value = "ID";
            sheet.Cells[1, 2].Value = "Type";
            sheet.Cells[1, 3].Value = "Recipient Email";
            sheet.Cells[1, 4].Value = "Message";
            sheet.Cells[1, 5].Value = "Sent At";

            using (var range = sheet.Cells[1, 1, 1, 5])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkRed);
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            var logs = _context.Notifications.AsQueryable();
            if (month.HasValue && year.HasValue)
                logs = logs.Where(l => l.SentAt.Month == month && l.SentAt.Year == year);

            var list = logs.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var l = list[i];
                sheet.Cells[i + 2, 1].Value = l.Id;
                sheet.Cells[i + 2, 2].Value = l.Type;
                sheet.Cells[i + 2, 3].Value = l.RecipientEmail;
                sheet.Cells[i + 2, 4].Value = l.Message;
                sheet.Cells[i + 2, 5].Value = l.SentAt.ToString("dd/MM/yyyy hh:mm tt");
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}