using System.Text;
using FindYourStoryApp.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace FindYourStoryApp.Controllers
{
    public class AdminUserController : Controller
    {


        FirebaseAuthProvider _authProvider;

        //Creates an instance of HttpClient that is used to send HTTP requests to the specified base address for the API.
        //Medium (2019) demonstrates creating an HttpClient with the base address.
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7148")
        };

        public AdminUserController()
        {
            //FreeCode Spot (2020) demonstrates initializing the FirebaseAuthProvider and FirebaseConfig with the
            //web API key of our Firebase project.
            _authProvider = new FirebaseAuthProvider(
                new FirebaseConfig("AIzaSyDN5ZLELI3vBMkUQgECLLiDsoVXRPnc0qQ"));
        }


        //Index() method retrieves all users from the database and displays them in the Index view.
        //Sends a GET request to the API to get all users.
        //If successful, deserializes the response, returns the Index view with a list of users.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        public async Task<IActionResult> Index()
        {
            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            //Medium (2019) demonstrates the Http GET request process.
            HttpResponseMessage response = await httpClient.GetAsync("api/AdminUserController/GetAllUsers");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserViewModel>>(jsonResponse);
                return View(users);
            }
            else
            {
                return BadRequest("Failed to retrieve users.");
            }
        }

        //Create() method retrieves the role IDs required for creating a new user.
        //Sends a GET request to the API to get the role IDs.
        //If successful, deserializes response, and populates the ViewData with them.
        //Returns the Create view with the ViewData containing role IDs.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        public async Task<IActionResult> Create()
        {

            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            //Medium (2019) demonstrates the Http GET request process.
            HttpResponseMessage response = await httpClient.GetAsync("api/AdminUserController/GetCreate");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var roleIds = JsonConvert.DeserializeObject<List<int>>(jsonResponse);

                //Panchal (2015) demonstrates how to populate a DropDownList with ViewData. 
                ViewData["RoleId"] = new SelectList(roleIds);
                return View();
            }
            else
            {
                return BadRequest("Failed to retrieve role IDs.");
            }
        }


        //Create() method handles the creation of a new user.
        //Binds the specified properties received from the form submission.
        //Initiates Firebase authentication to register and log in the user using provided email and password.
        //Handles any Firebase authentication exceptions that may occur during registration and login processes.
        //If registration and login are successful, retrieves the User UID from Firebase Authentication services.
        //Stores the User UID in a session variable if available.
        //If registration or login fails, sets an error message in the ViewBag for display.
        //Tries to add the user to the database.
        //If successful, redirects to the Index action.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string FirstName, string LastName, string Email, DateOnly Dob, string Username, string Password, int RoleId)
        {

            string token = "";

            //////////////////////////////////////FIREBASE AUTHENTICATION////////////////////////////////////////////////////
            //FreeCode Spot (2020) demonstrates registering a user for Firebase Authentication.
            //TRY...CATCH block handles any Firebase exceptions that occur during registration and login processes.
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
                //Deserializes the Firebase Exception ResponseData into the FirebaseError object.
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);

                ViewBag.RegError = firebaseEx.error.message;
            }

            //////////////////////////////////////ADD USER TO DATABASE////////////////////////////////////////////////////

            try
            {
                Models.User user;
                user = Models.User.Create(FirstName, LastName, Dob, Email, Username, RoleId, token);

                //Medium (2019) demonstrates the Http POST request process.
                StringContent jsonContent = new(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("api/AdminUserController/PostUser", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //Edit() method retrieves a user by its ID from the database for editing.
        //Sends a GET request to the API to get the data associated with the user ID.
        //If successful, deserializes  response,  retrieves role IDs for the view, and returns the Edit view with the user details.
        //If no user is found for the given ID, returns a NotFound response.
        public async Task<IActionResult> Edit(int? id)
        {

            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            if (id == null)
            {
                return NotFound();
            }

            //Medium (2019) demonstrates the Http GET request process.
            HttpResponseMessage userResponse = await httpClient.GetAsync("api/AdminUserController/GetEditUser?userId=" + id);

            if (userResponse.IsSuccessStatusCode)
            {
                var jsonResponseUser = await userResponse.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Models.User>(jsonResponseUser);

                //Medium (2019) demonstrates the Http GET request process.
                HttpResponseMessage responseRole = await httpClient.GetAsync("api/AdminUserController/GetCreate");
                if (responseRole.IsSuccessStatusCode)
                {
                    var jsonResponseRoles = await responseRole.Content.ReadAsStringAsync();
                    var roleIds = JsonConvert.DeserializeObject<List<int>>(jsonResponseRoles);

                    //Panchal (2015) demonstrates how to populate a DropDownList with ViewData. 
                    ViewData["RoleId"] = new SelectList(roleIds);

                    return View(user);
                }
                else
                {
                    return BadRequest("Failed to retrieve role IDs.");
                }

            }
            else
            {
                return NotFound();
            }
        }

        //Edit() method handles updating an existing user in the database.
        //Binds the specified properties, converts to JSON, serializes the object, and sends a PUT request to the API to update the user.
        //If successful, retrieves role IDs for the view, then redirects to the Index action.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Dob,Email,Username,RoleId,FirebaseUid")] Models.User user)
        {

            try
            {
                //Medium (2019) demonstrates the Http PUT request process.
                StringContent jsonContent = new(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                HttpResponseMessage responseUser = await httpClient.PutAsync("api/AdminUserController/UpdateUser", jsonContent);

                if (responseUser.IsSuccessStatusCode)
                {

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    //Medium (2019) demonstrates the Http GET request process.
                    HttpResponseMessage responseRole = await httpClient.GetAsync("api/AdminUserController/GetCreate");
                    if (responseRole.IsSuccessStatusCode)
                    {
                        var jsonResponseRoles = await responseRole.Content.ReadAsStringAsync();
                        var roleIds = JsonConvert.DeserializeObject<List<int>>(jsonResponseRoles);

                        ViewBag.RoleId = new SelectList(roleIds, user.RoleId);

                        return View(user);
                    }
                    else
                    {
                        return BadRequest("Failed to retrieve role IDs.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delete() method retrieves a user by its ID from the database for deletion.
        //Sends a GET request to the API to get the data associated with the user ID.
        //If successful, deserializes response, returns the Delete view with the user details.
        //If no user is found for the given ID, returns a NotFound response.
        public async Task<IActionResult> Delete(int? id)
        {


            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            if (id == null)
            {
                return NotFound();
            }

            //Medium (2019) demonstrates the Http GET request process.
            HttpResponseMessage response = await httpClient.GetAsync("api/AdminUserController/GetDeleteUser?userId=" + id);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Models.User>(jsonResponse);
                return View(user);
            }
            else
            {
                return NotFound();
            }

        }

        //DeleteConfirmed() method handles the deletion of a user from the database.
        //Sends a DELETE request to the API to delete the user with the specified ID.
        //If successful, redirects to the Index action.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            //Medium (2019) demonstrates the Http DELETE request process.
            HttpResponseMessage response = await httpClient.DeleteAsync("api/AdminUserController/DeleteUser?userId=" + id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("Failed to delete role.");
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
                HttpResponseMessage response = await httpClient.GetAsync("/api/HomeController/GetUserDetails?firebaseUid=" + currentUser);

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
                HttpResponseMessage response = await httpClient.GetAsync("/api/HomeController/GetUserDetails?firebaseUid=" + currentUser);

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

    }
}
//REFERENCE LIST:
//Medium. 2019. Consuming a web API using a typed HttpClient — .NET Core, 19 May 2019 (Version 1.0)
//[Source code] https://medium.com/cheranga/calling-web-apis-using-typed-httpclients-net-core-20d3d5ce980
//(Accessed 25 March 2024).
//Panchal, H. 2015. Various Ways To Populate Dropdownlist in MVC. C# Corner, 25 May 2015 (Version 1.0)
//[Source code] https://www.c-sharpcorner.com/UploadFile/32bcb2/different-ways-for-populating-dropdownlist-in-mvc/
//(Accessed 18 March 2024).
