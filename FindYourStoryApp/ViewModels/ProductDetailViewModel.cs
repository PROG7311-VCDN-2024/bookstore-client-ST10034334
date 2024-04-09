namespace FindYourStoryApp.DTOs
{
    //ProductDetailViewModel represents the view model for displaying the product details for the book catalogue.
    public class ProductDetailViewModel
    {
        public int ProductId { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int Price { get; set; }

        public int InStock { get; set; }
    }
}
