using System.Text;
using FindYourStoryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FindYourStoryApp.Controllers
{
    public class AdminRoleController : Controller
    {

        //Creates an instance of HttpClient that is used to send Http requests to the specified base address for the API.
        //Medium (2019) demonstrates creating an HttpClient with the base address.
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7148")
        };

        //Index() method retrieves all roles from the database and displays them in the Index view.
        //Sends a GET request to the API to get all roles.
        //If successful, deserializes the response, returns the Index view with a list of roles.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        public async Task<IActionResult> Index()
        {
            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());


            //Medium (2019) demonstrates the Http GET request process.
            HttpResponseMessage response = await httpClient.GetAsync("api/AdminRoleController/GetAllRoles");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<Role>>(jsonResponse);
                return View(roles);
            }
            else
            {
                return BadRequest("Failed to retrieve roles.");
            }

        }

        //Create() method returns the Create view for creating a new role.
        //Returns the Create view.
        public IActionResult Create()
        {
            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            return View();
        }


        //Create() method handles the creation of a new role.
        //Binds the specified properties, converts to JSON, serializes object, and sends a POST request to the API
        //to add the role to the database.
        //If successful, redirects to the Index action.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleType")] Role role)
        {

            try
            {
                role = Role.Create(role.RoleType);

                //Medium (2019) demonstrates the Http POST request process.
                StringContent jsonContent = new(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("api/AdminRoleController/PostRole", jsonContent);

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

        //Edit() method retrieves a role by its ID from the database for editing.
        //Sends a GET request to the API to get the data associated with the role ID.
        //If successful, deserializes  response, and returns the Edit view with the role details.
        //If no role is found for the given ID, returns a NotFound response.
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
            HttpResponseMessage response = await httpClient.GetAsync("api/AdminRoleController/GetEditRole?roleId=" + id);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var role = JsonConvert.DeserializeObject<Role>(jsonResponse);

                return View(role);
            }
            else
            {
                return NotFound();
            }

        }


        //Edit() method handles updating an existing role in the database.
        //Binds the specified properties, converts to JSON, serializes object, and sends a PUT request to the API to update the role.
        //If successful, redirects to the Index action.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleType")] Role role)
        {

            try
            {
                //Medium (2019) demonstrates the Http PUT request process.
                StringContent jsonContent = new(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync("api/AdminRoleController/UpdateRole", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(role);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        //Delete() method retrieves a role by its ID from the database for deletion.
        //Sends a GET request to the API to get the data associated with the role ID.
        //If successful, deserializes response, returns the Delete view with the role details.
        //If no role is found for the given ID, returns a NotFound response.
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
            HttpResponseMessage response = await httpClient.GetAsync("api/AdminRoleController/GetDeleteRole?roleId=" + id);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var role = JsonConvert.DeserializeObject<Role>(jsonResponse);

                return View(role);
            }
            else
            {
                return NotFound();
            }

        }

        //DeleteConfirmed() method handles the deletion of a role from the database.
        //Sends a DELETE request to the API to delete the role with the specified ID.
        //If successful, redirects to the Index action.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Medium (2019) demonstrates the Http DELETE request process.
            HttpResponseMessage response = await httpClient.DeleteAsync("api/AdminRoleController/DeleteRole?roleId=" + id);

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
                HttpResponseMessage response = await httpClient.GetAsync("/api/HomeController/GetUserDetails?firebaseUid=" + currentUser);

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
    }
}
//REFERENCE LIST:
//Medium. 2019. Consuming a web API using a typed HttpClient — .NET Core, 19 May 2019 (Version 1.0)
//[Source code] https://medium.com/cheranga/calling-web-apis-using-typed-httpclients-net-core-20d3d5ce980
//(Accessed 25 March 2024).