using System.Text;
using FindYourStoryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace FindYourStoryApp.Controllers
{
    public class AdminOrderController : Controller
    {

        //Creates an instance of HttpClient that is used to send HTTP requests to the specified base address for the API.
        //Medium (2019) demonstrates creating an HttpClient with the base address.
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7148")
        };


        //Index() method retrieves all orders from the database and displays them in the Index view.
        //Sends a GET request to the API to get all orders.
        //If successful, deserializes the response, returns the Index view with a list of orders.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        public async Task<IActionResult> Index()
        {

            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            //Medium (2019) demonstrates the Http GET request process.
            HttpResponseMessage response = await httpClient.GetAsync("api/AdminOrderController/GetAllOrders");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderViewModel>>(jsonResponse);
                return View(orders);
            }
            else
            {
                return BadRequest("Failed to retrieve orders.");
            }
        }

        //Create() method retrieves the order IDs required for creating a new order.
        //Sends a GET request to the API to get the order IDs.
        //If successful, deserializes response, and populates the ViewData with them.
        //Returns the Create view with the ViewData containing order IDs.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        public async Task<IActionResult> Create()
        {

            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            //Medium (2019) demonstrates the Http GET request process.
            HttpResponseMessage responseUser = await httpClient.GetAsync("api/AdminOrderController/GetUserCreate");

            if (responseUser.IsSuccessStatusCode)
            {
                var jsonResponseUser = await responseUser.Content.ReadAsStringAsync();
                var userIds = JsonConvert.DeserializeObject<List<int>>(jsonResponseUser);

                //Panchal (2015) demonstrates how to populate a DropDownList with ViewData. 
                ViewData["UserId"] = new SelectList(userIds);


                //Medium (2019) demonstrates the Http GET request process.
                HttpResponseMessage responseShipAddress = await httpClient.GetAsync("api/AdminOrderController/GetShippingAddressCreate");

                if (responseShipAddress.IsSuccessStatusCode)
                {
                    var jsonResponseShipAddress = await responseShipAddress.Content.ReadAsStringAsync();
                    var shipAddressIds = JsonConvert.DeserializeObject<List<int>>(jsonResponseShipAddress);

                    //Panchal (2015) demonstrates how to populate a DropDownList with ViewData. 
                    ViewData["ShippingAddressId"] = new SelectList(shipAddressIds);

                    return View();
                }

                else
                {
                    return BadRequest("Failed to retrieve ship address IDs.");
                }
            }
            else
            {
                return BadRequest("Failed to retrieve user IDs.");
            }
        }


        //Create() method handles the creation of a new order.
        //Binds the specified properties, converts to JSON, serializes object, and sends a POST request to the API
        //to add the order to the database.
        //If successful, redirects to the Index action.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,UserId,OrderDate,PaymentStatus,ShippingAddressId,TotalAmount")] Order order)
        {
            try
            {
                order = Order.Create(order.UserId, order.OrderDate, order.PaymentStatus, order.ShippingAddressId, order.TotalAmount);

                //Medium (2019) demonstrates the Http POST request process.
                StringContent jsonContent = new(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("api/AdminOrderController/PostOrder", jsonContent);



                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        //Edit() method retrieves a order by its ID from the database for editing.
        //Sends a GET request to the API to get the data associated with the order ID.
        //If successful, deserializes  response,  retrieves order IDs for the view, and returns the Edit view with the order details.
        //If no order is found for the given ID, returns a NotFound response.
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
            HttpResponseMessage orderResponse = await httpClient.GetAsync("api/AdminOrderController/GetEditOrder?orderId=" + id);

            if (orderResponse.IsSuccessStatusCode)
            {
                var jsonResponseOrder = await orderResponse.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Models.Order>(jsonResponseOrder);

                //Medium (2019) demonstrates the Http GET request process.
                HttpResponseMessage responseUser = await httpClient.GetAsync("api/AdminOrderController/GetUserCreate");

                if (responseUser.IsSuccessStatusCode)
                {
                    var jsonResponseUser = await responseUser.Content.ReadAsStringAsync();
                    var userIds = JsonConvert.DeserializeObject<List<int>>(jsonResponseUser);

                    //Panchal (2015) demonstrates how to populate a DropDownList with ViewData. 
                    ViewData["UserId"] = new SelectList(userIds);


                    //Medium (2019) demonstrates the Http GET request process.
                    HttpResponseMessage responseShipAddress = await httpClient.GetAsync("api/AdminOrderController/GetShippingAddressCreate");

                    if (responseShipAddress.IsSuccessStatusCode)
                    {
                        var jsonResponseShipAddress = await responseShipAddress.Content.ReadAsStringAsync();
                        var shipAddressIds = JsonConvert.DeserializeObject<List<int>>(jsonResponseShipAddress);

                        //Panchal (2015) demonstrates how to populate a DropDownList with ViewData. 
                        ViewData["ShippingAddressId"] = new SelectList(shipAddressIds);

                        return View(order);
                    }

                    else
                    {
                        return BadRequest("Failed to retrieve ship address IDs.");
                    }
                }
                else
                {
                    return BadRequest("Failed to retrieve user IDs.");
                }

            }
            else
            {
                return NotFound();
            }
        }

        //Edit() method handles updating an existing order in the database.
        //Binds the specified properties, converts to JSON, serializes object, and sends a PUT request to the API to update the order.
        //If successful, redirects to the Index action.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,UserId,OrderDate,PaymentStatus,ShippingAddressId,TotalAmount")] Order order)
        {
            try
            {
                //Medium (2019) demonstrates the Http PUT request process.
                StringContent jsonContent = new(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync("api/AdminOrderController/UpdateOrder", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(order);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        //Delete() method retrieves a order by its ID from the database for deletion.
        //Sends a GET request to the API to get the data associated with the order ID.
        //If successful, deserializes response, returns the Delete view with the order details.
        //If no order is found for the given ID, returns a NotFound response.
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
            HttpResponseMessage response = await httpClient.GetAsync("api/AdminOrderController/GetDeleteOrder?orderId=" + id);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Models.Order>(jsonResponse);


                return View(order);
            }
            else
            {
                return NotFound();
            }
        }


        //DeleteConfirmed() method handles the deletion of a order from the database.
        //Sends a DELETE request to the API to delete the order with the specified ID.
        //If successful, redirects to the Index action.
        //If an error occurs during the process, returns a BadRequest response with an error message.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Medium (2019) demonstrates the Http DELETE request process.
            HttpResponseMessage response = await httpClient.DeleteAsync("api/AdminOrderController/DeleteOrder?orderId=" + id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("Failed to delete order.");
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
//Panchal, H. 2015. Various Ways To Populate Dropdownlist in MVC. C# Corner, 25 May 2015 (Version 1.0)
//[Source code] https://www.c-sharpcorner.com/UploadFile/32bcb2/different-ways-for-populating-dropdownlist-in-mvc/
//(Accessed 18 March 2024).