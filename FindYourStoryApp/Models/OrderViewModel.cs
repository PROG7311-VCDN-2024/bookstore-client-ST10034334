namespace FindYourStoryApp.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public string User { get; set; }

        public DateOnly OrderDate { get; set; }

        public string PaymentStatus { get; set; } = null!;

        public string ShippingAddress { get; set; }

        public int TotalAmount { get; set; }

    }
}
