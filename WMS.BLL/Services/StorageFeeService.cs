using System;
using System.Collections.Generic;
using System.Text;
using WMS.BLL.Interfaces;
using WMS.DAL;
using WMS.Models;

namespace WMS.BLL.Services
{
    
    public class StorageFeeService : IStorageFeeService
    {
        private readonly AppDbContext _context;
        private const double RatePerHourPerKg = 0.5;

        public StorageFeeService(AppDbContext context)
        {
            _context = context;
        }

        public StorageFee Calculate(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null) return null;

            var exitDate = product.ExitDate ?? DateTime.Now;
            double hoursStored = (exitDate - product.EntryDate).TotalHours;
            double totalFee = hoursStored * product.WeightKg * RatePerHourPerKg;

            var existing = _context.StorageFees.FirstOrDefault(s => s.ProductId == productId);

            if (existing != null)
            {
                // تحديث الموجود مش عمل جديد
                existing.TotalFee = Math.Round(totalFee, 4);
                existing.CalculatedAt = DateTime.Now;
            }
            else
            {
                // عمل جديد لو مش موجود
                existing = new StorageFee
                {
                    ProductId = productId,
                    RatePerHourPerKg = RatePerHourPerKg,
                    TotalFee = Math.Round(totalFee, 2),
                    CalculatedAt = DateTime.Now
                };
                _context.StorageFees.Add(existing);
            }

            _context.SaveChanges();
            return existing;
        }

        public StorageFee GetByProductId(int productId)
        {
            return _context.StorageFees
                .FirstOrDefault(s => s.ProductId == productId);
        }

        public List<StorageFee> GetAll()
        {
            return _context.StorageFees.ToList();
        }
    }
}
