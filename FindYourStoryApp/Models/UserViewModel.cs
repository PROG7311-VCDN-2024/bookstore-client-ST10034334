namespace FindYourStoryApp.Models
{
    //UserViewModel represents the view model for displaying users.
    public class UserViewModel
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string RoleType { get; set; }

        public string FirebaseUid { get; set; } = null!;

    }
}
