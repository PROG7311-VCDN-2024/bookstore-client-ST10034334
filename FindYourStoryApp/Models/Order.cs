namespace FindYourStoryApp.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateOnly OrderDate { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public int ShippingAddressId { get; set; }

    public int TotalAmount { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ShippingAddress ShippingAddress { get; set; } = null!;

    //Create() method creates a new instance of Order with the provided parameters.
    //It sets the properties of the Order object based on the provided values.
    //Returns the newly created Order object.
    public static Order Create(int userId, DateOnly orderDate, string paymentStatus, int shipAddressId, int totalAmount)
    {

        return new Order
        {
            UserId = userId,
            OrderDate = orderDate,
            PaymentStatus = paymentStatus,
            ShippingAddressId = shipAddressId,
            TotalAmount = totalAmount
        };
    }

}
