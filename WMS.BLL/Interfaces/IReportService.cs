namespace WMS.BLL.Interfaces
{
    public interface IReportService
    {
        byte[] GenerateProductsReport(int? month = null, int? year = null);
        byte[] GenerateStorageFeesReport(int? month = null, int? year = null);
        byte[] GenerateLogsReport(int? month = null, int? year = null);
    }
}