namespace WMS.Models
{
    public class StorageFee
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public double RatePerHourPerKg { get; set; } = 0.5;
        public double TotalFee { get; set; }
        public DateTime CalculatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public Product Product { get; set; }
    }
}