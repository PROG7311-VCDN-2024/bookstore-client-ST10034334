namespace FindYourStoryApp.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public byte[] BookCoverImage { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int Price { get; set; }

    public int InStock { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();


    //Create() method creates a new instance of Product with the provided parameters.
    //It sets the properties of the Product object based on the provided values.
    //Returns the newly created Product object.
    public static Product Create(byte[] bookCoverImage, string title, string author, int price, int inStock)
    {

        return new Product
        {
            BookCoverImage = bookCoverImage,
            Title = title,
            Author = author,
            Price = price,
            InStock = inStock
        };
    }
}
