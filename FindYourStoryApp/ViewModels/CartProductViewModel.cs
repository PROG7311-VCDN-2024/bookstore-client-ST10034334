namespace FindYourStoryApp.DTOs
{
    //CartProductViewModel represents the view model for displaying the product details of the cart items for the cart.
    public class CartProductViewModel
    {
        public int ProductId { get; set; }

        public string Title { get; set; } = null!;

        public int Quantity { get; set; }

        public int TotalAmount { get; set; }
    }
}
