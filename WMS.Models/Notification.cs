using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string RecipientEmail { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // "Entry", "Exit", "Shipping"
        public DateTime SentAt { get; set; } = DateTime.Now;

        // Navigation Property
        public Product Product { get; set; }
    }
}
