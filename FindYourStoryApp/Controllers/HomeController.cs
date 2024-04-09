using System.Diagnostics;
using System.Text;
using FindYourStoryApp.DTOs;
using FindYourStoryApp.Models;
using FindYourStoryApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FindYourStoryApp.Controllers
{
    public class HomeController : Controller
    {

        //Creates an instance of HttpClient that is used to send HTTP requests to the specified base address for the API.
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7148")
        };

        //Index() method retrieves the top 8 books from the Product table via the GetBookCatalogue() API method.
        //It asynchronously sends a GET request to the GetBookDetails() API method using HttpClient.
        //Upon receiving a successful response, it reads the response as a string and deserializes it into a list of products.
        //The method then separates the book images (as a string list) and details (as a list of ProductDTO objects),
        //converting images to Base64String for display.
        //ViewBags are used to pass necessary data to the View, and the appropriate View is returned.
        public async Task<IActionResult> Index()
        {
            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            HttpResponseMessage response = await httpClient.GetAsync("/api/HomeController/GetBookCatalogue");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                List<Product>? deserialisedResponse = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);


                List<string> bookCovers = new List<string>();
                List<ProductDetailViewModel> bookDetails = new List<ProductDetailViewModel>();

                foreach (var book in deserialisedResponse)
                {
                    //Microsoft (2024) demonstrates how to use the Convert.ToBase64String() method.
                    string imgData = Convert.ToBase64String(book.BookCoverImage);
                    bookCovers.Add(imgData);
                }

                foreach (var book in deserialisedResponse)
                {
                    ProductDetailViewModel product = new ProductDetailViewModel
                    {
                        ProductId = book.ProductId,
                        Title = book.Title,
                        Author = book.Author,
                        Price = book.Price,
                        InStock = book.InStock
                    };
                    bookDetails.Add(product);
                }

                ViewBag.ImageDataList = bookCovers;
                ViewBag.BookDetailList = bookDetails;
            }
            else
            {
                return View("NoBooks");
            }

            return View("Index");
        }


        //BookDetails() method retrieves the details of a specific book from the API based on the provided product ID.
        //It asynchronously sends a GET request to the GetBookDetails() API method using HttpClient.
        //Upon receiving a successful response, it reads the response as a string and deserializes it into a list of products.
        //The method then separates the book images (as a string list) and details (as a list of ProductDTO objects),
        //converting images to Base64String for display.
        //ViewBags are used to pass necessary data to the View, and the appropriate View is returned.
        public async Task<IActionResult> BookDetails(int productId)
        {

            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            HttpResponseMessage response = await httpClient.GetAsync("/api/HomeController/GetBookDetails?productID=" + productId);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                List<Product>? deserialisedResponse = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);


                List<string> bookCover = new List<string>();
                List<ProductDetailViewModel> bookDetail = new List<ProductDetailViewModel>();

                foreach (var book in deserialisedResponse)
                {
                    //Microsoft (2024) demonstrates how to use the Convert.ToBase64String() method.
                    string imgData = Convert.ToBase64String(book.BookCoverImage);
                    bookCover.Add(imgData);
                }

                foreach (var book in deserialisedResponse)
                {
                    ProductDetailViewModel product = new ProductDetailViewModel
                    {
                        ProductId = book.ProductId,
                        Title = book.Title,
                        Author = book.Author,
                        Price = book.Price,
                        InStock = book.InStock
                    };
                    bookDetail.Add(product);
                }

                ViewBag.ImageDataList = bookCover;
                ViewBag.BookDetailList = bookDetail;
            }
            else
            {
                return View("NoBooks");
            }

            return View("BookDetails");
        }

        //SearchBooks() method retrieves the details of a specific book from the API based on the provided title search query.
        //It asynchronously sends a GET request to the SearchBookCatalogue() API method using HttpClient.
        //Upon receiving a successful response, it reads the response as a string and deserializes it into a list of products.
        //The method then separates the book images (as a string list) and details (as a list of ProductDTO objects),
        //converting images to Base64String for display.
        //ViewBags are used to pass necessary data to the View, and the appropriate View is returned.
        public async Task<IActionResult> SearchBooks(string searchQuery)
        {
            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            HttpResponseMessage response = await httpClient.GetAsync("/api/HomeController/SearchBookCatalogue?searchQuery=" + searchQuery);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                List<Product>? deserialisedResponse = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);


                List<string> bookCover = new List<string>();
                List<ProductDetailViewModel> bookDetail = new List<ProductDetailViewModel>();

                foreach (var book in deserialisedResponse)
                {
                    //Microsoft (2024) demonstrates how to use the Convert.ToBase64String() method.
                    string imgData = Convert.ToBase64String(book.BookCoverImage);
                    bookCover.Add(imgData);
                }

                foreach (var book in deserialisedResponse)
                {
                    ProductDetailViewModel product = new ProductDetailViewModel
                    {
                        ProductId = book.ProductId,
                        Title = book.Title,
                        Author = book.Author,
                        Price = book.Price,
                        InStock = book.InStock
                    };
                    bookDetail.Add(product);
                }

                ViewBag.ImageDataList = bookCover;
                ViewBag.BookDetailList = bookDetail;
            }
            else
            {
                return View("NoBooks");
            }

            return View("SearchBooks");
        }



        //AddBookToCart() method handles the addition of a book to the cart.
        //It uses the session token to authorize access to pages.
        //If the token is present, it creates a Cart entry based on the provided product ID, quantity, and amount.
        //It then sends a POST request to the PostBookToCart() API method with the cart entry data.
        //If the request is successful, it sets TempData indicating the book was added to the cart and redirects to the Home/Index page.
        //If the request fails or the token is missing, redirects to the Home/Index page or the User/Login page respectively.
        [HttpPost]
        public async Task<IActionResult> AddBookToCart(int productId, int quantity, int amount)
        {
            //FreeCode Spot (2020) demonstrates using the session token to authorise access to pages.
            var token = HttpContext.Session.GetString("UserLoggedIn");

            if (token != null)
            {
                Cart cartEntry;
                cartEntry = Cart.Create(userIntegerID(userLoggedIn()), productId, quantity, amount);

                StringContent jsonContent = new(JsonConvert.SerializeObject(cartEntry), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("api/HomeController/PostBookToCart?productAmount=" + amount, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
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


        //ViewCart() method displays the user's cart based on the provided user ID.
        //It uses the session token to authorize access to pages.
        //If the token is present, it asynchronously sends a GET request to the GetCart() API method using HttpClient.
        //Upon receiving a successful response, it reads the response as a string and deserializes it into a list of CartViewModel objects.
        //The method then separates the book images (as a string list) and details (as a list of CartProductViewModel objects),
        //converting images to Base64String for display.
        //Calculates the total cart price by summing up the total amounts of all products in the cart.
        //ViewBags are used to pass necessary data to the View, and the appropriate View is returned.
        //If token is missing or invalid, redirects to the User/Login page.
        public async Task<IActionResult> ViewCart()
        {
            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            var totalCartPrice = 0;

            //FreeCode Spot (2020) demonstrates using the session token to authorise access to pages.
            var token = HttpContext.Session.GetString("UserLoggedIn");

            if (token != null)
            {

                HttpResponseMessage response = await httpClient.GetAsync("/api/HomeController/GetCart?userID=" + userIntegerID(userLoggedIn()));

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    List<CartViewModel>? deserialisedResponse = JsonConvert.DeserializeObject<List<CartViewModel>>(jsonResponse);


                    List<string> bookCover = new List<string>();
                    List<CartProductViewModel> bookDetail = new List<CartProductViewModel>();

                    foreach (var book in deserialisedResponse)
                    {
                        //Microsoft (2024) demonstrates how to use the Convert.ToBase64String() method.
                        string imgData = Convert.ToBase64String(book.BookCoverImage);
                        bookCover.Add(imgData);
                    }

                    foreach (var book in deserialisedResponse)
                    {
                        CartProductViewModel product = new CartProductViewModel
                        {
                            ProductId = book.ProductId,
                            Title = book.Title,
                            Quantity = book.Quantity,
                            TotalAmount = book.TotalAmount
                        };
                        bookDetail.Add(product);
                    }

                    foreach (var book in deserialisedResponse)
                    {

                        totalCartPrice += book.TotalAmount;
                    }


                    ViewBag.ImageDataList = bookCover;
                    ViewBag.BookDetailList = bookDetail;
                    ViewBag.TotalPrice = totalCartPrice;

                    return View("ViewCart");
                }
                else
                {
                    return View("NoBooks");
                }
            }

            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        //RemoveBookFromCart() method handles the removal of a book from the user's cart.
        //It uses the session token to authorize access to pages.
        //If the token is present, it sends a POST request to the PostRemoveBookFromCart() API method with the product ID and user ID.
        //If the request is successful, it sets TempData indicating the book was removed from the cart and redirects to the Home/Index page.
        //If the request fails or the token is missing, redirects to the Home/Index page or the User/Login page respectively.
        [HttpPost]
        public async Task<IActionResult> RemoveBookFromCart(int productId)
        {
            //FreeCode Spot (2020) demonstrates using the session token to authorise access to pages.
            var token = HttpContext.Session.GetString("UserLoggedIn");

            if (token != null)
            {
                HttpResponseMessage response = await httpClient.PostAsync("api/HomeController/PostRemoveBookFromCart?productId=" + productId + "&userId=" + userIntegerID(userLoggedIn()), null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["BookRemoved"] = "Book Removed From Cart!";
                    return RedirectToAction("ViewCart");
                }
                else
                {
                    TempData["BookRemoved"] = "Book Removed From Cart Failed!";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public async Task<IActionResult> Checkout()
        {
            //Assigns the user currently logged in's email and role to ViewBags.
            ViewBag.UserLoggedIn = userStringEmail(userEmail());
            ViewBag.UserRole = userStringRole(userRole());

            int userID = userIntegerID(userLoggedIn());
            bool deserialisedResponse = false;

            HttpResponseMessage response = await httpClient.GetAsync("/api/HomeController/GetShippingAddressExists?userId=" + userID);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                deserialisedResponse = JsonConvert.DeserializeObject<bool>(jsonResponse);
            }

            if (deserialisedResponse == true)
            {
                return RedirectToAction("ProcessPayment", "MockPayment");
            }
            else
            {
                return View("AddShippingAddress");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddShippingAddress(string AddressLine1, string AddressLine2, string City, string State, string PostalCode, string Country)
        {

            //FreeCode Spot (2020) demonstrates using the session token to authorise access to pages.
            var token = HttpContext.Session.GetString("UserLoggedIn");

            if (token != null)
            {
                ShippingAddress shippingAddress;
                shippingAddress = ShippingAddress.Create(userIntegerID(userLoggedIn()), AddressLine1, AddressLine2, City, State, PostalCode, Country);

                StringContent jsonContent = new(JsonConvert.SerializeObject(shippingAddress), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("api/HomeController/PostShippingAddress", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ShipAddressAdded"] = "Shipping Address Added!";
                    return RedirectToAction("ProcessPayment", "MockPayment");
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
                HttpResponseMessage response = await httpClient.GetAsync("/api/HomeController/GetUserDetails?firebaseUid=" + currentUser);

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//REFERENCE LIST:
//Cull, B. 2016. Using Sessions and HttpContext in ASP.NET Core and MVC Core, 23 July 2016 (Version 1.0)
//[Source code] https://bencull.com/blog/using-sessions-and-httpcontext-in-aspnetcore-and-mvc-core
//(Accessed 28 February 2024).
//Dot Net Tutorials. 2024. LINQ Sum Method in C#, 2024 (Version 1.0)
//[Source code] https://dotnettutorials.net/lesson/linq-sum-method/
//(Accessed 5 March 2024).
//FreeCode Spot. 2020. How to Integrate Firebase in ASP NET Core MVC, 2020 (Version 1.0)
//[Source code] https://www.freecodespot.com/blog/firebase-in-asp-net-core-mvc/
//(Accessed 8 March 2024).
//Gideon. 2011. LINQ query to select top five. Stack Overflow, 2 February 2011 (Version 1.0)
//[Source code] https://stackoverflow.com/questions/4872946/linq-query-to-select-top-five
//(Accessed 26 January 2024).
//Microsoft. 2023. Anonymous types, 29 November 2023 (Version 1.0)
//[Source code]https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
//(Accessed 26 January 2024).
//Microsoft. 2024. Convert.ToBase64String Method (Version 1.0)
//[Source code] https://learn.microsoft.com/en-us/dotnet/api/system.convert.tobase64string?view=net-8.0
//(Accessed 26 January 2024).
//Microsoft. 2024. DbSet.FindAsync Method, 2024 (Version 1.0)
//[Source code] https://learn.microsoft.com/en-us/dotnet/api/system.data.entity.dbset.findasync?view=entity-framework-6.2.0
//(Accessed 10 March 2024).
//TutorialsTeacher. 2023. Filtering Operator - Where (Version 1.0)
//[Source code] https://www.tutorialsteacher.com/linq/linq-filtering-operators-where
//(Accessed 21 November 2023).