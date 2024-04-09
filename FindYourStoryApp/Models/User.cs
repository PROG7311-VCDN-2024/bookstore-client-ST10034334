namespace FindYourStoryApp.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int RoleId { get; set; }

    public string FirebaseUid { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();


    //Create() method creates a new instance of User with the provided parameters.
    //It sets the properties of the User object based on the provided values.
    //Returns the newly created User object.
    public static User Create(string firstName, string lastName, DateOnly dob, string email, string username, int roleId, string firebaseUid)
    {

        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Dob = dob,
            Email = email,
            Username = username,
            RoleId = roleId,
            FirebaseUid = firebaseUid
        };
    }

}
