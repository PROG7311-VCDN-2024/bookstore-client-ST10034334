using System.Text;
using FindYourStoryApp.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FindYourStoryApp.Controllers
{
    public class UserController : Controller
    {
        //Declared private fields to store the DB context of Find Your Story and the FirebaseAuthProvider.
        //FreeCode Spot (2020) demonstrates declaring the FirebaseAuthProvider.
        FirebaseAuthProvider _authProvider;

        //Declared private fields to store the log information and the DB context of Find Your Story.
        private readonly ILogger<UserController> _logger;


        //The UserController Constructor() takes in an instance of the DB context and the FirebaseAuthProvider and
        //assigns them to appropriate private fields.
        public UserController(IConfiguration configuration, ILogger<UserController> logger)
        {
            _logger = logger;

            //Microsoft (2023) demonstrates how to use Configuration.
            string apiKey = configuration["Firebase:APIKey"];

            //FreeCode Spot (2020) demonstrates initializing the FirebaseAuthProvider and FirebaseConfig with the
            //web API key of our Firebase project.
            _authProvider = new FirebaseAuthProvider(
                new FirebaseConfig(apiKey));
            ;
        }



        //Creates an instance of HttpClient that is used to send HTTP requests to the specified base address for the API.
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7148")
        };


        //Register() method handles displaying the registration form view.
        //Returns the registration view.
        public IActionResult Register()
        {
            return View();
        }


        //Register() method handles the registration of a new user.
        //It receives parameters for user details.
        //Performs Firebase authentication to register the user with the provided email and password.
        //If successful, retrieves the user's authentication token and stores it in the session.
        //If an error occurs during Firebase authentication, retrieves the error message and displays it in the ViewBag.
        //Creates a new user object with the provided details and the obtained token.
        //Converts the user object to JSON format and sends a POST request to the PostUser() API method with the user data.
        //If the request is successful, sets TempData indicating successful user creation and redirects to the Home/Index page.
        //If the request fails, or if there's an error during Firebase authentication, redirects to the Home/Index page.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string FirstName, string LastName, string Email, DateOnly Dob, string Username, string Password)
        {
            string token = "";

            //////////////////////////////////////FIREBASE AUTHENTICATION////////////////////////////////////////////////////
            //FreeCode Spot (2020) demonstrates registering a user for Firebase Authentication.
            try
            {
                await _authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                var authoriseFB = await _authProvider.SignInWithEmailAndPasswordAsync(Email, Password);


                token = authoriseFB.User.LocalId;

                if (token != null)
                {
                    HttpContext.Session.SetString("UserLoggedIn", token);
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);

                ViewBag.RegError = firebaseEx.error.message;
            }

            //////////////////////////////////////ADD USER TO DATABASE////////////////////////////////////////////////////
            Models.User user;

            //All users are set to have a role of id 2:user when registering, as only admin can make users admin.
            user = Models.User.Create(FirstName, LastName, Dob, Email, Username, 2, token);
            _logger.LogInformation("Token" + token);

            StringContent jsonContent = new(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("api/UserController/PostUser", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["UserAdded"] = "User Created!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //Login() method handles displaying the login form view.
        //Returns the login view.
        public IActionResult Login()
        {
            return View();
        }


        //Login() method handles the user login process.
        //It receives parameters for user email and password.
        //Performs Firebase authentication by attempting to sign in the user with the provided email and password.
        //If successful, retrieves the user's authentication token and stores it in the session.
        //Redirects to the Home/Index page upon successful login.
        //If an error occurs during Firebase authentication, retrieves the error message and displays it in the ViewBag.
        //Calls the getFalseLogin() method to handle failed login attempts, passing the email as a parameter.
        //Returns the login view if authentication fails or if no error occurs.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {

            //FreeCode Spot (2020) demonstrates logging in a user for Firebase Authentication.
            try
            {
                var authoriseFB = await _authProvider.SignInWithEmailAndPasswordAsync(Email, Password);

                string token = authoriseFB.User.LocalId;

                if (token != null)
                {
                    HttpContext.Session.SetString("UserLoggedIn", token);

                    return RedirectToAction("Index", "Home");
                }

            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ViewBag.LoginError = firebaseEx.error.message;
                getFalseLogin(Email, firebaseEx);
            }

            return View();
        }

        //getFalseLogin() method handles logging of failed login attempts.
        //It receives the user's email and FirebaseError message as a parameter.
        //Attempts to append the details of the failed login attempt to a text file named "FalseLoginAttempts.txt".
        //If successful, writes the email, timestamp, and error occured of the failed login attempt to the text file.
        //If an error occurs during the file writing process, logs the error message using the LogError() method provided by the logger.
        private void getFalseLogin(string email, FirebaseError error)
        {
            //Wells (2019) demonstrates how to write and append to a textfile.
            try
            {
                using (StreamWriter writer = new StreamWriter("FalseLoginAttempts.txt", append: true))
                {
                    writer.WriteLine($"A False Login Occurred By {email} On {DateTime.Now}  :   Error Occured: {error}");
                }

            }
            //Microsoft (2024) demonstrates the LogError() method.
            catch (Exception ex)
            {
                _logger.LogError($"An error occured logging a false login: {ex.Message}");
            }
        }

        //UserProfile() method handles displaying the user profile.
        //Retrieves the logged-in user's ID using the userLoggedIn() method.
        //Sends a GET request to the GetUserProfile() API method with the user ID.
        //If the request is successful, returns the user profile view with the response data.
        //If the request fails, sets TempData indicating that the user profile was not found and redirects to the Home/Index page.
        public async Task<IActionResult> UserProfile()
        {

            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());


            HttpResponseMessage response = await httpClient.GetAsync("api/UserController/GetUserProfile?userId=" + userIntegerID(userLoggedIn()));

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                List<Models.User>? deserialisedResponse = JsonConvert.DeserializeObject<List<Models.User>>(jsonResponse);
                return View(deserialisedResponse);
            }
            else
            {
                TempData["UserProfile"] = "User proifle not found!";
                return RedirectToAction("Index", "Home");
            }

        }


        //UserProfile() method handles updating the user profile.
        //Receives a user object with updated profile details.
        //Converts the user object to JSON format and sends a POST request to the PostUserProfile() API method.
        //If the request is successful, sets TempData indicating successful user update.
        //Retrieves the updated user profile using a GET request to the GetUserProfile() API method.
        //If the user profile retrieval is successful, deserializes the response JSON and returns the UserProfile view with the updated user details.
        //If the user profile retrieval fails, sets TempData indicating that the user profile was not found and redirects to the Home/Index page.
        //If the request to update the user profile fails, sets TempData indicating failed user update and redirects to the Home/Index page.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProfile([Bind("UserId,FirstName,LastName,Dob,Email,Username,RoleId,FirebaseUid")] Models.User user)
        {

            StringContent jsonContent = new(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            HttpResponseMessage responsePost = await httpClient.PostAsync("api/UserController/PostUserProfile", jsonContent);

            if (responsePost.IsSuccessStatusCode)
            {
                TempData["UserUpdated"] = "User Updated!";

                HttpResponseMessage responseGet = await httpClient.GetAsync("api/UserController/GetUserProfile?userId=" + userIntegerID(userLoggedIn()));

                if (responseGet.IsSuccessStatusCode)
                {
                    var jsonResponse = await responseGet.Content.ReadAsStringAsync();
                    List<Models.User>? deserialisedResponse = JsonConvert.DeserializeObject<List<Models.User>>(jsonResponse);
                    return View(deserialisedResponse);
                }
                else
                {
                    TempData["UserProfile"] = "User proifle not found!";
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                TempData["UserUpdated"] = "User Updated Failed!";
                return RedirectToAction("Index", "Home");
            }

        }


        //Logout() method handles logging out the user.
        //Removes the "UserLoggedIn" session variable to log out the user.
        //Redirects to the Home/Index page after logout.
        public IActionResult Logout()
        {
            //FreeCode Spot (2020) demonstrates logging out a user.
            HttpContext.Session.Remove("UserLoggedIn");
            return RedirectToAction("Index", "Home");
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
                HttpResponseMessage response = await httpClient.GetAsync("/api/UserController/GetUserDetails?firebaseUid=" + currentUser);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    List<Models.User>? deserialisedResponse = JsonConvert.DeserializeObject<List<Models.User>>(jsonResponse);

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
                HttpResponseMessage response = await httpClient.GetAsync("/api/UserController/GetUserDetails?firebaseUid=" + currentUser);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    List<Models.User>? deserialisedResponse = JsonConvert.DeserializeObject<List<Models.User>>(jsonResponse);

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
        //It follows a similar process as userLoggedIn() HomeController method to retrieve user details and extracts the user ID.
        //Returns the user ID of the logged-in user.
        public async Task<int> userLoggedIn()
        {
            var currentUser = HttpContext.Session.GetString("UserLoggedIn");
            var userID = 0;

            //Checks if a user is logged in.
            if (currentUser != null)
            {
                HttpResponseMessage response = await httpClient.GetAsync("/api/UserController/GetUserDetails?firebaseUid=" + currentUser);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    List<Models.User>? deserialisedResponse = JsonConvert.DeserializeObject<List<Models.User>>(jsonResponse);

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
//REFERNCE LIST:
//Code Maze. 2022. How to secure passwords with BCrypt.NET, 19 December 2022 (Version 2.0)
//[Source code] https://code-maze.com/dotnet-secure-passwords-bcrypt/
//(Accessed 25 February 2024).
//Cull, B. 2016. Using Sessions and HttpContext in ASP.NET Core and MVC Core, 23 July 2016 (Version 1.0)
//[Source code] https://bencull.com/blog/using-sessions-and-httpcontext-in-aspnetcore-and-mvc-core
//(Accessed 28 February 2024).
//FreeCode Spot. 2020. How to Integrate Firebase in ASP NET Core MVC, 2020 (Version 1.0)
//[Source code] https://www.freecodespot.com/blog/firebase-in-asp-net-core-mvc/
//(Accessed 8 March 2024).
//Microsoft. 2023. Configuration in ASP.NET Core, 9 November 2023 (Version 1.0)
//[Source code] https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0
//(Accessed 12 March 2024).
//Microsoft. 2024. DbSet.FindAsync Method, 2024 (Version 1.0)
//[Source code] https://learn.microsoft.com/en-us/dotnet/api/system.data.entity.dbset.findasync?view=entity-framework-6.2.0
//(Accessed 10 March 2024).
//Microsoft. 2024. LoggerExtensions.LogError Method, 2024 (Version 1.0)
//[Source code] https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerextensions.logerror?view=dotnet-plat-ext-8.0
//(Accessed 29 February 2024)
//TutorialsTeacher. 2023. Filtering Operator - Where (Version 1.0)
//[Source code] https://www.tutorialsteacher.com/linq/linq-filtering-operators-where
//(Accessed 25 February 2024).
//TutorialsTeacher. 2023. Projection Operators: Select, SelectMany (Version 1.0)
//[Source code] https://www.tutorialsteacher.com/linq/linq-projection-operators
//(Accessed 25 February 2024).
//Wells, B. 2019. Write to Text File C#, 17 June 2019 (Version 1.0)
//[Source code] https://wellsb.com/csharp/beginners/write-to-text-file-csharp
//(Accessed 29 February 2024)