using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double WeightKg { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }
        public string Status { get; set; } // "InWarehouse" or "Delivered"
        public DateTime EntryDate { get; set; } = DateTime.Now;
        public DateTime? ExitDate { get; set; }
        // Navigation Properties
        public StorageFee StorageFee { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }
}
