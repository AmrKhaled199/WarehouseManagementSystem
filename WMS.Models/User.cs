using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // "Admin" or "Staff"
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
