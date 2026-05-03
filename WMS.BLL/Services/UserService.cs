using System;
using System.Collections.Generic;
using System.Text;
using WMS.BLL.Interfaces;
using WMS.DAL;
using WMS.Models;

namespace WMS.BLL.Services
{
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;
        public UserService (AppDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }
        public User? GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public bool ChangeRole(int id, string newRole)
        {
            var user = GetById(id);
            if (user == null) return false;

            // نتأكد إن الـ Role صح
            if (newRole != "Admin" && newRole != "Staff") return false;

            user.Role = newRole;
            _context.SaveChanges();
            return true;
        }

    }
}
