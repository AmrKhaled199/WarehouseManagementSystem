using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using WMS.BLL.Interfaces;
using WMS.DAL;
using WMS.Models;

namespace WMS.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public NotificationService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _smtpHost = configuration["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? "587");
            _senderEmail = configuration["EmailSettings:SenderEmail"] ?? "";
            _senderPassword = configuration["EmailSettings:SenderPassword"] ?? "";
        }

        private async Task SendEmail(string toEmail, string subject, string body, string type, int productId)
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_senderEmail, _senderPassword)
            };

            var message = new MailMessage(_senderEmail, toEmail, subject, body);
            await client.SendMailAsync(message);

            _context.Notifications.Add(new Notification
            {
                ProductId = productId,
                RecipientEmail = toEmail,
                Message = body,
                Type = type,
                SentAt = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }

        public async Task SendEntryEmail(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null) return;
            await SendEmail(product.SenderEmail, "✅ تم استلام منتجك", $"مرحباً {product.SenderName}, تم استلام '{product.Name}'.", "Entry", productId);
        }

        public async Task SendExitEmail(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null) return;
            await SendEmail(product.SenderEmail, "📤 تم شحن منتجك", $"مرحباً {product.SenderName}, تم شحن '{product.Name}'.", "Exit", productId);
        }

        public async Task SendShippingEmail(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null) return;
            await SendEmail(product.ReceiverEmail, "🚚 منتجك في الطريق", $"مرحباً {product.ReceiverName}, المنتج '{product.Name}' في الطريق.", "Shipping", productId);
        }
    }
}