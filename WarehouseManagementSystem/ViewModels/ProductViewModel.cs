using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.ViewModels
{
    public class ProductViewModel
    {
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(0.01, 3000, ErrorMessage = "The weight must be between 0.1 and 3000 kg")]
        public double WeightKg { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Receiver name is required")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Receiver email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ReceiverEmail { get; set; }

        [Required(ErrorMessage = "Sender name is required")]
        public string SenderName { get; set; }
        [Required(ErrorMessage = "Sender email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string SenderEmail { get; set; }
    }
}
