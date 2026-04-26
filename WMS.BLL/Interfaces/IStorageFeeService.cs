using System;
using System.Collections.Generic;
using System.Text;
using WMS.Models;

namespace WMS.BLL.Interfaces
{
    public interface IStorageFeeService
    {
        StorageFee Calculate(int productId);
        StorageFee GetByProductId(int productId);
        List<StorageFee> GetAll();
    }
}
