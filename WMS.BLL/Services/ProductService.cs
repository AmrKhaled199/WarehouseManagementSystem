using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using WMS.BLL.Interfaces;
using WMS.DAL;
using WMS.Models;

namespace WMS.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private const int MaxItems = 1000;
        private const double MaxWeightTons = 3;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }
        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }


        // بيتحقق إن عدد المنتجات مش وصل 1000
        // بيتحقق إن إجمالي الوزن مش هيتعدى 3000 كيلو(3 طن)
        // لو الاتنين تمام بيحفظ تاريخ الدخول ويحط الـ Status على InWarehouse
        // بيحفظ في الـ Database ويرجع true
        public bool Add(Product product)
        {
            // Validation على الـ Capacity
            if (GetTotalCount() >= MaxItems)
                return false;

            if (GetTotalWeight() + product.WeightKg > MaxWeightTons * 1000)
                return false;

            product.EntryDate = DateTime.Now;
            product.Status = "InWarehouse";

            _context.Products.Add(product);
            _context.SaveChanges();
            return true;
        }


        // بيغير حالة المنتج، ولو الحالة بقت Delivered بيحفظ تاريخ الخروج تلقائياً.
        public bool UpdateStatus(int id, string status)
        {
            var product = GetById(id);
            if (product == null) return false;

            product.Status = status;

            if (status == "Delivered")
                product.ExitDate = DateTime.Now;

            _context.SaveChanges();
            return true;
        }

        // بيحذف المنتج من الـ Database لو موجود، ويرجع true. لو مش موجود يرجع false.
        public bool Delete(int id)
        {
            var product = GetById(id);
            if (product == null) return false;
            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }

        // بيحسب إجمالي الوزن الحالي للمنتجات اللي في المخزن (اللي حالتها InWarehouse) ويرجع الرقم.
        public double GetTotalWeight()
        {
            return _context.Products
                .Where(p => p.Status == "InWarehouse")
                .Sum(p => p.WeightKg);
        }

        // بيحسب إجمالي عدد المنتجات اللي في المخزن (اللي حالتها InWarehouse) ويرجع الرقم.
        public int GetTotalCount()
        {
            return _context.Products
                 .Count(p => p.Status == "InWarehouse");
        }

        double IProductService.GetTotalCount()
        {
            return GetTotalCount();
        }
    }
}
