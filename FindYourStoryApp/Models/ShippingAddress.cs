namespace FindYourStoryApp.Models;

public partial class ShippingAddress
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string AddressLine2 { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();



    //Create() method creates a new instance of ShippingAddressDTO with the provided parameters.
    //It sets the properties of the ShippingAddressDTO object based on the provided values.
    //Returns the newly created ShippingAddressDTO object.
    public static ShippingAddress Create(int userId, string addressLine1, string addressLine2, string city, string state, string postalCode, string country)
    {

        return new ShippingAddress
        {
            UserId = userId,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            City = city,
            State = state,
            PostalCode = postalCode,
            Country = country
        };
    }
}
