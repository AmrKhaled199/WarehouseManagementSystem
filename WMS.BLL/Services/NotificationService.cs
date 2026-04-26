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
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SenderEmail = "fmobile098@gmail.com";
        private const string SenderPassword = "vtfj imrk lhlc aafv";

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        private async Task SendEmail(string toEmail, string subject, string body, string type, int productId)
        {
            using var client = new SmtpClient(SmtpHost, SmtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(SenderEmail, SenderPassword)
            };

            var message = new MailMessage(SenderEmail, toEmail, subject, body);
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