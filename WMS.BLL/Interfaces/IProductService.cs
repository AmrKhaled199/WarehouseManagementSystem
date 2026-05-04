using System;
using System.Collections.Generic;
using System.Text;
using WMS.Models;

namespace WMS.BLL.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product? GetById(int id);
        bool Add(Product product);
        bool UpdateStatus(int id, string status);
        bool Delete(int id);
        double GetTotalWeight();
        int GetTotalCount();
        List<Product> Search(string query);

    }
}
