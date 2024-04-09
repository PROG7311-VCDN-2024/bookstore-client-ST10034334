using FindYourStoryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FindYourStoryApp.Controllers
{
    public class OrderController : Controller
    {

        //Creates an instance of HttpClient that is used to send HTTP requests to the specified base address for the API.
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7148")
        };


        public async Task<IActionResult> ViewOrder()
        {
            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            //FreeCode Spot (2020) demonstrates using the session token to authorise access to pages.
            var token = HttpContext.Session.GetString("UserLoggedIn");

            if (token != null)
            {
                HttpResponseMessage response = await httpClient.GetAsync("api/OrderController/ViewOrder?userId=" + userIntegerID(userLoggedIn()));

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    List<Order> deserialisedResponse = JsonConvert.DeserializeObject<List<Order>>(jsonResponse);

                    return View("ViewOrder", deserialisedResponse);
                }
                else
                {
                    return View("NoOrders");
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
                HttpResponseMessage response = await httpClient.GetAsync("/api/OrderController/GetUserDetails?firebaseUid=" + currentUser);

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
                HttpResponseMessage response = await httpClient.GetAsync("/api/OrderController/GetUserDetails?firebaseUid=" + currentUser);

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
                HttpResponseMessage response = await httpClient.GetAsync("/api/OrderController/GetUserDetails?firebaseUid=" + currentUser);

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
//Cull, B. 2016. Using Sessions and HttpContext in ASP.NET Core and MVC Core, 23 July 2016 (Version 1.0)
//[Source code] https://bencull.com/blog/using-sessions-and-httpcontext-in-aspnetcore-and-mvc-core
//(Accessed 28 February 2024).
