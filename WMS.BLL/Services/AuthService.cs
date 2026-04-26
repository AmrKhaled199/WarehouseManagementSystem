using WMS.BLL.Interfaces;
using WMS.DAL;
using WMS.Models;

namespace WMS.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public User Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (user == null) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            return isValid ? user : null;
        }

        public bool Register(User user, string password)
        {
            if (IsEmailExists(user.Email)) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }

        public bool IsEmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}