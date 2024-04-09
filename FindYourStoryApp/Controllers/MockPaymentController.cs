using System.Globalization;
using System.Text;
using FindYourStoryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FindYourStoryApp.Controllers
{
    public class MockPaymentController : Controller
    {

        //Creates an instance of HttpClient that is used to send HTTP requests to the specified base address for the API.
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7148")
        };

        //ProcessPayment() method is an action method that first retrieves the total cart amount for
        //the user currently logged in and through using object initialization, sets the Amount value of 
        //the payment to that total cart amount.
        //User email and Role ViewBags are added here so that the user's details appear in menu bar.
        //Lastly passes this specific mockPayment with this specific total cart amount to the View.
        public async Task<IActionResult> ProcessPayment()
        {
            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            HttpResponseMessage response = await httpClient.GetAsync("/api/MockPaymentController/GetOrderAmount?userId=" + userIntegerID(userLoggedIn()));

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                int deserialisedResponse = JsonConvert.DeserializeObject<int>(jsonResponse);

                var mockPayment = new MockPayment
                {
                    Amount = deserialisedResponse
                };

                return View("ProcessPayment", mockPayment);
            }

            return View("ProcessPayment");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //ProcessPayment() is an action method that takes in the necessary parameters for the MockPayment class and performs
        //error handling before showing the PaymentSuccessful View.
        //Lastly, passes an error message to the View and redirects to the ProcessPayment View.
        public IActionResult ProcessPayment(string CardNumber, string ExpiryDate, string CVVDigits, int Amount)
        {

            if (ModelState.IsValid)
            {
                if (ValidExpiryDate(ExpiryDate) == true && !string.IsNullOrEmpty(CardNumber) && !string.IsNullOrEmpty(CVVDigits)
                    && Amount >= 0)
                {

                    return View("PaymentSuccessful");
                }

            }

            ViewBag.ErrorMessage = "Payment was not successful. Please check payment details.";
            return RedirectToAction("ProcessPayment", "MockPayment");

        }

        //ValidExpiryDate() method makes sure that the expiry date entered in by user is in the correct "MM/YY" format.
        public bool ValidExpiryDate(string expDate)
        {
            //Microsoft (2024) demonstrates the DateTime.TryParseExact() method.
            DateTime validDate;
            return DateTime.TryParseExact(expDate, "MM/yy", null, DateTimeStyles.None, out validDate); ;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOrders()
        {
            //FreeCode Spot (2020) demonstrates using the session token to authorise access to pages.
            var token = HttpContext.Session.GetString("UserLoggedIn");

            if (token != null)
            {

                HttpResponseMessage responseOrder = await httpClient.PostAsync("api/MockPaymentController/PostCheckOrders?userId=" + userIntegerID(userLoggedIn()), null);

                if (responseOrder.IsSuccessStatusCode)
                {
                    TempData["OrderAdded"] = "Order Added!";

                    HttpResponseMessage responseCart = await httpClient.PostAsync("api/MockPaymentController/RemoveAllFromCart?userId=" + userIntegerID(userLoggedIn()), null);

                    if (responseCart.IsSuccessStatusCode)
                    {
                        TempData["CartRemoved"] = "Cart Refreshed!";
                    }
                    else
                    {
                        TempData["CartRemoved"] = "Cart Refreshed Failed!";
                    }

                    return RedirectToAction("ViewOrder", "Order");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }


        //userEmail() method retrieves the email of the user currently logged in.
        //It first checks if a user is logged in by retrieving the session token.
        //If a user is logged in, it sends a GET request to the GetUserDetails() API method with the Firebase UID.
        //Upon successful retrieval of user details, it deserializes the response into a list of User objects.
        //It then extracts the email from the deserialized response and returns it.
        //If the user is not logged in or an error occurs during retrieval, it returns an empty string.
        public async Task<string> userEmail()
        {
            var currentUser = HttpContext.Session.GetString("UserLoggedIn");
            var email = "";

            //Checks if a user is logged in.
            if (currentUser != null)
            {
                HttpResponseMessage response = await httpClient.GetAsync("/api/MockPaymentController/GetUserDetails?firebaseUid=" + currentUser);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    List<User>? deserialisedResponse = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

                    foreach (var user in deserialisedResponse)
                    {
                        email = user.Email;

                    }
                    return email;
                }

            }
            return email;
        }


        //userStringEmail() method retrieves the email of the user as a string.
        //It extracts the email from the Task<string> parameter and returns it.
        public string userStringEmail(Task<string> _email)
        {
            string email = _email.Result;
            return email;
        }


        //userRole() method retrieves the role ID of the user currently logged in.
        //It follows a similar process as userEmail() to retrieve user details and extracts the role ID.
        //Returns the role ID of the user.
        public async Task<string> userRole()
        {
            var currentUser = HttpContext.Session.GetString("UserLoggedIn");
            var role = "";

            //Checks if a user is logged in.
            if (currentUser != null)
            {
                HttpResponseMessage response = await httpClient.GetAsync("/api/MockPaymentController/GetUserDetails?firebaseUid=" + currentUser);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    List<User>? deserialisedResponse = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

                    foreach (var user in deserialisedResponse)
                    {
                        role = user.RoleId.ToString();

                    }
                    return role;
                }

            }
            return role;
        }

        //userStringRole() method retrieves the role ID of the user as a string.
        //It extracts the role ID from the Task<string> parameter and returns it.
        public string userStringRole(Task<string> _role)
        {
            string role = _role.Result;
            return role;
        }

        //userLoggedIn() method retrieves the ID of the user currently logged in.
        //It follows a similar process as userEmail() to retrieve user details and extracts the user ID.
        //Returns the user ID of the logged-in user.
        public async Task<int> userLoggedIn()
        {
            var currentUser = HttpContext.Session.GetString("UserLoggedIn");
            var userID = 0;

            //Checks if a user is logged in.
            if (currentUser != null)
            {
                HttpResponseMessage response = await httpClient.GetAsync("/api/MockPaymentController/GetUserDetails?firebaseUid=" + currentUser);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    List<User>? deserialisedResponse = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

                    foreach (var user in deserialisedResponse)
                    {
                        userID = user.UserId;

                    }
                    return userID;
                }

            }
            return userID;
        }

        //userIntegerID() method retrieves the user ID of the user as an integer.
        //It extracts the user ID from the Task<int> parameter and returns it.
        public int userIntegerID(Task<int> _id)
        {
            int id = _id.Result;
            return id;
        }
    }
}
//REFERENCE LIST:
//Dot Net Tutorials. 2024. LINQ Sum Method in C#, 2024 (Version 1.0)
//[Source code] https://dotnettutorials.net/lesson/linq-sum-method/
//(Accessed 5 March 2024).
//Microsoft. 2024. DateTime.TryParseExact Method, 2024 (Version 1.0).
//[Source code] https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tryparseexact?view=net-8.0
//(Accessed 5 March 2024).