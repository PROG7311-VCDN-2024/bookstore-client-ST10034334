namespace FindYourStoryApp.ViewModels
{
    //CartViewModel represents the view model for displaying cart items.
    public class CartViewModel
    {
        public int ProductId { get; set; }

        public byte[] BookCoverImage { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int Quantity { get; set; }

        public int TotalAmount { get; set; }

    }
}
