
<h3 align="center">PROG7311 SPRINT 3</h3>
<h3>READ ME FILE</h3>
<h3>ST10034334 + ST10245621</h3>
<br><br>


<h3>TABLE OF CONTENTS</h3>
• Project Description <br>
• The Technologies Used And Why	 <br>
• How To Install Find Your Story (Via Github)  	<br>
• How To Run/Use Find Your Story Website: Customer Point Of View  <br>
• How To Run/Use Find Your Story Website: Admin Point Of View  <br>


<br>
<h2>PROJECT DESCRIPTION:</h2> <br>
PROJECT TITLE: FIND YOUR STORY <br>
Discover a whole new way to shop for your favourite books with Find Your Story! Our platform offers an intuitive and user-friendly experience for everyone involved, including Customers and Admins. <br>
Explore a vast library of books, complete with detailed descriptions and eye-catching cover images. With our smart search feature, finding the perfect book is as easy as typing in the title or even just a part of it. <br>
Admins have full control over the platform, from adding new books to adjusting prices and managing user accounts. Customers can create their own profiles, browse through books, and add them to their cart for easy checkout through our secure payment gateway. <br>
We are committed to delivering your favourite books to you in an easy and user-friendly manner. Join us and bring your personal library home with ease! <br><br>

<h2>THE TECHNOLOGIES USED AND WHY:</h2> <br>
The technologies used to create Find Your Story are the following: <br>
1. Visual Studio 2022. <br>
2. ASP.NET Core Web App (Model-View-Controller). <br>
3. C# Class Library. <br>
4. SSMS (SQL Server Management Studio). <br>
5. Local Browser (Microsoft Edge). <br>
6. Firebase Console. <br>

Visual Studio 2022 was used as the IDE to code this website in C#. <br><br>
ASP.NET Core Web App (Model-View-Controller) was used to create Find Your Story in the form of a website using models, views, and controllers.<br><br>
C# Class library was used to store all the models relating to the classes used in the Find your Story application as well as the DBContext for interacting with the database.<br><br>
SSMS (SQL Server Management Studio) was used to create the SQL database and necessary tables that stores all the information for the Find Your Story application (user data, including registration information, login credentials (through ASP.NET Core Identity), product data, as well as cart data).<br><br>
Firebase Console was used to handle user authentication and stores data such as email, password, firebase UID, etc., to allow users to register, log in, and log out efficiently.<br><br>
 
<h2>HOW TO INSTALL FIND YOUR STORY (VIA GITHUB):</h2> <br>
** This guide on installation uses a sample application, namely “ShoppingCart App” ** <br>
Step 1: Copy the link to the GitHub repository and paste it into the top search bar in your browser. <br>
Step 2: Click on the green "Code" button and select the option to "Download ZIP". <br>

<p align="center">
  <img src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/b186ac13-db48-4fef-91c7-bf7dc6dbcb1b">
</p>


Step 3: A download should begin shortly and appear at the bottom of the screen. <br>
Step 4: Click on that now downloaded ZIP file at the bottom of the screen and a page will open. <br>
Step 5: Click on the "Extract All" option at the top of this page.<br>



![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/2e94f4ad-1f4d-458f-b334-2747fff50f37)<br>


Step 6: A pop-up will be shown, and you can select where you would like this file to be saved through clicking the "browse" button or you may leave it as the default. <br>


![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/04beb54b-1626-49af-9ef6-ac939d686ac8)<br>


Step 7: Next, you will click the "extract" button at the bottom of the pop-up, which will begin to extract all the files and paste them in the location that you have selected.<br>


![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/2a07de30-bc02-467b-afce-d1920838e17b)<br>


Step 8: Once the extraction is complete, a page will show the extracted file. Proceed to click on this now extracted file.
Step 9: Once clicked, you will see a few more files shown. The file that you need to double-click on is the one with the little Visual Studio sign and with the .sln file type.<br>

![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/b4223966-e82d-4b38-8f1b-2a5574c55916)<br>


Step 10: Once Double-Clicked, Visual Studio will now open with the application, and lastly, click on the Run button located at the top of Visual Studio to run the program.<br>


![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/8e286d86-b9f0-40dc-af09-1347ee93d743)<br><br>


<h2>HOW TO RUN/USE FIND YOUR STORY WEBSITE: CUSTOMER POINT OF VIEW:</h2><br>

Home Page: Find Your Story will begin by showing you a home page, as shown below. The user is presented with a collection of books which includes, a view of the book cover and details associated with each book such as title, author, price, and stock availability. The user is also presented with a “Details” button which allows users to click on and view even more options associated with the selected book.<br>
The home page also includes a search feature, which allows users to search for books by entering in the title of the book, or even just a part of the title.  <br>
The user must click on the “Register” or “Login” navigation menu item. If the user does not login or register, the user is unable to view their cart/add to their cart or view their orders. <br>
In addition, the user will be presented with the Login Page, shown below, if they try to access their cart without having an account. <br>

![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/c8e69078-8421-46ff-812f-db0ae1de9864)<br>

<img width="627" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/4275b568-7f06-4654-bbff-1d328b0ed7d0"><br>



Register Page: Once the “Register’ menu item is clicked, the Register Page will be shown to the user, as shown below. The user must enter in the required details for the registration. Error messages will be displayed for wrong input. As default, users are assigned the role of a customer and only an admin can change the user role. <br>
Once the user has entered in their registration details, the user must click on the “Register” button. This user and their details will be saved to the local database as well as in Firebase Console to allow the user profile to be managed locally and the authentication managed through an online service. Once registered, the user will be automatically logged in.<br>



<img width="606" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/20bddc93-7a7d-4abc-ab29-17661b7beace"><br>


<img width="607" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/1c5b5e2b-88d2-4d8e-a87a-c735d5f6bcd3"><br>


<img width="743" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/f0b37857-e8f3-436f-b34d-c464bedbeda6"><br>


Login Page: Once the “Log In’ menu item is clicked, the Login Page will be shown to the user, as shown below. The user must enter in the required details for the login. Error messages will be displayed for wrong input. Once the user has entered in their login details successfully, the user must click on the “Login” button. This login authentication is also handled through Firebase Console. <br>
The user will now have the ability to use the website to add books to their cart, view their cart, and their orders. Once logged in, the user will be presented with the Home Page again. In addition, if the user has failed to login, a false login attempt will be written to a text file along with the email of the user committing this false login attempt and the date and time on which this occurred for analysing purposes. This recording of false logins was completed through using the logger service which follows the singleton design pattern. <br>


<img width="627" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/4275b568-7f06-4654-bbff-1d328b0ed7d0"><br>


<img width="604" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/20db04c6-465b-4c99-b43d-0ab9193fd76b"><br>

![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/098e32b2-5c45-41e9-baee-e85db8992924)<br>




User Profile Page: Once the “Profile Icon” menu item is clicked, the user can either choose the “View Profile” item or “Log Out” item. The User Profile Page, as shown below, allows users to view their personal details as well as update them as they so wish. The only field that they are unable to change is their email address as this links with Firebase Console. The “Log Out” item simply logs out the user.<br>


![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/14a11684-6c27-4dcb-bf08-b7410165b777)<br>


![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/cb7b3b7a-da9d-4aa1-9f8e-da84827448e1)<br>

Product Details Page: When the user clicks on any book item, the Product Details Page will be shown to the user, as shown below.  This page is where the user is able to view the book details, enter in the book quantity (how many copies of the book they would like to order), and the option to add this book to their cart by clicking on the “Add To Cart” button. Once the user has added their books to their cart, they can now click on the shopping cart icon in the navigation bar to view their cart.<br>


![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/e1bbc1ba-e77e-4f89-a33c-d6b579c6ec1b)<br>


Cart Page: Once the shopping cart item is clicked, the Cart Page will be shown to the user, as shown below. Here the user is able to view a list of all the book items that they have added to their cart. It also includes the option of removing book items from the cart if the user so pleases. In addition, after the list of books, a total is displayed to the user (this total is made up of the total quantity ordered for each book). Once the user has viewed their cart, they can either continue shopping by navigating back to the Home page or they can click on the “Checkout” button once completed shopping.<br>

<img width="923" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/1a978c7c-d3b3-4207-b3bb-ad787ba29a17"><br>


<img width="928" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/f7ee9a6b-1a55-45cc-ad5f-0dd1ee405b18"><br>



<img width="925" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/f30f02f3-9e8c-439d-ab06-70a88be1e687"><br>


Shipping Address Page: Once the “Checkout” button is clicked, the Shipping Address Page will be shown to user for their first-time ordering, as shown below. The user must enter in the required details for their shipping address. Error messages will be displayed for wrong input. Once the user has entered in their shipping address details successfully, the user must click on the “Submit” button.<br>

<img width="608" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/c52e5c31-753c-451e-b6aa-127407ec51d2"><br>

Payment Page: Once the “Submit” button is clicked, the Payment Page will be shown to the user, as shown below. The user must enter in their card details and click on the “Pay Now” button to make their book order. Because this is a mock payment, the Successful Payment Page will be shown to the user after successfully entering payment details. In addition, once the mock payment has been made, the quantity in stock of those products ordered will be updated (decreased).<br>


<img width="628" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/cad5b9d2-4afe-4100-b28b-950fa259b789"><br>

<img width="925" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/1f553116-71ae-4311-880d-82776f1f85c8"><br>


View Order Page: Once the order has been placed, the user is able to view their list of orders, as shown below, by either clicking on the “View Order” button in the Successful Payment Page or by clicking on the “Orders” menu item. <br>


<img width="919" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/808bed01-c3cb-4ef1-9c6c-c2da3408ce75"><br><br>


<h2>HOW TO RUN/USE FIND YOUR STORY WEBSITE: ADMIN POINT OF VIEW:</h2><br>
Firstly, the admin is able to do everything that was explained above from the customer’s point of view, in the sense of ordering books. But by being an admin, they will have the ability to do much more than just ordering books. All the functionalities that an admin can do are listed below, supported with images.<br>

An Admin has the following privileges:<br>
•	Create, edit, and delete users.<br>

 ![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/b7b72d26-541e-4d3e-8e74-b09ce920136e)<br>


•	Create, edit, and delete book products. <br>
 ![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/b6a454c5-b286-454c-94ac-475f4ae00b0a)<br>


o	When adding a new product, the book cover image is able to be uploaded from files, as shown below.<br>

<img width="959" alt="image" src="https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/4a421952-0db4-4f80-b323-d00a895b0432"><br>

	
•	Create, edit, and delete orders.<br>

![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/4747c941-95b9-473b-a125-a117f6b921ce)<br>


•	Create, edit, and delete roles.<br>

![image](https://github.com/PROG7311-VCDN-2024/bookstore-client-ST10034334/assets/101701375/511aeca9-8a2f-4b09-84a1-ed102bd250cf)<br>





