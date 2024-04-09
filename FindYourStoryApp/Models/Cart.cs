namespace FindYourStoryApp.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int TotalAmount { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public static Cart Create(int userId, int productId, int quantity, int totalAmount)
    {
        return new Cart
        {
            UserId = userId,
            ProductId = productId,
            Quantity = quantity,
            TotalAmount = totalAmount

        };
    }
}
