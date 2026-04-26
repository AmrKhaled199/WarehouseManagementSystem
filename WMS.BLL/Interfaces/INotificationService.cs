using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.BLL.Interfaces
{
    public interface INotificationService
    {
        Task SendEntryEmail(int productId);
        Task SendExitEmail(int productId);
        Task SendShippingEmail(int productId);
    }
}