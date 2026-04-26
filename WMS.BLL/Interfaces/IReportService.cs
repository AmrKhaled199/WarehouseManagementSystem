using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.BLL.Interfaces
{
    public interface IReportService
    {
        byte[] GenerateProductsReport();
        byte[] GenerateStorageFeesReport();
        byte[] GenerateLogsReport();
    }
}
