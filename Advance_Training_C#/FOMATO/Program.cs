using FOMATO.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Serialization;

namespace FOMATO
{
    class Program
    {
        // Simple lists to store data (instead of database for now)
        static List<User> users = new List<User>();
        static List<Restaurant> restaurants = new List<Restaurant>();
        static List<Order> orders = new List<Order>();
        static Cart currentCart;
        //static User currentUser;
        static User currUser = new User();
        static string connectionString = @"Data Source=LAPTOP-M86ENGOL;Initial Catalog=FomatoDB;Trusted_Connection=True;";
        static void Main(string[] args)
        {
            Console.WriteLine("=== Welcome to FOMATO Food Delivery ===");
            ShowAuthenticationMenu();
        }
        static void ShowAuthenticationMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== FOMATO AUTHENTICATION ===");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (currUser.Login())
                        {
                            // If login successful, show main menu
                            InitializeSampleData();

                            if (currUser.currentUser.IsAdmin)
                            {
                                ShowAdminMenu();
                            }
                            else
                            {
                                ShowMainMenu();
                            }
                        }
                        break;
                    case "2":
                        User newUser = new User();
                        //newUser.Register();
                        newUser.CallStoredProcedureAdoNet(connectionString, "InsertUserSimple", "Sahil3", "Sahil3@gmail.com", "8995585349", "Sahil3@097");
                        break;
                    case "3":
                        Console.WriteLine("Thank you for using FOMATO!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static void ShowAdminMenu()
        {
            while (true)
            {
                Console.WriteLine($"\n=== FOMATO ADMIN PANEL ===");
                Console.WriteLine($"Welcome, Admin {currUser.currentUser.Name}!");
                Console.WriteLine("1. Manage Restaurants");
                Console.WriteLine("2. View All Orders");
                Console.WriteLine("3. View All Users");
                Console.WriteLine("4. View System Statistics");
                Console.WriteLine("5. Logout");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ManageRestaurants();
                        break;
                    case "2":
                        ViewAllOrders();
                        break;
                    case "3":
                        ViewAllUsers();
                        break;
                    case "4":
                        ViewSystemStatistics();
                        break;
                    case "5":
                        Console.WriteLine("Logged out successfully!");
                        currUser.currentUser = null;
                        currentCart = null;
                        ShowAuthenticationMenu();
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;

                }

            }
        }

        static void ManageRestaurants()
        {
            while (true)
            {
                Console.WriteLine("\n=== MANAGE RESTAURANTS ===");
                Console.WriteLine("1. Add New Restaurant");
                Console.WriteLine("2. View All Restaurants");
                Console.WriteLine("3. Add Food Item to Restaurant");
                Console.WriteLine("4. Back to Admin Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewRestaurant();
                        break;
                    case "2":
                        ViewAllRestaurantsAdmin();
                        break;
                    case "3":
                        AddFoodItemToRestaurant();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static void AddNewRestaurant()
        {
            Console.WriteLine("\n=== ADD NEW RESTAURANT ===");
            Console.Write("Enter Restaurant Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Cuisine Type: ");
            string cuisine = Console.ReadLine();

            Console.Write("Enter Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.WriteLine("Enter Address Details:");
            Console.Write("Street: ");
            string street = Console.ReadLine();

            Console.Write("City: ");
            string city = Console.ReadLine();

            Console.Write("State: ");
            string state = Console.ReadLine();

            Console.Write("Zip Code: ");
            string zipCode = Console.ReadLine();

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(cuisine))
            {
                var address = new Address(restaurants.Count + 1, street, city, state, zipCode, "India", "Restaurant");
                var restaurant = new Restaurant(restaurants.Count + 1, name, cuisine, address, phone, email);
                restaurants.Add(restaurant);
                Console.WriteLine($"\n🎉 Restaurant '{name}' added successfully!");
            }
            else
            {
                Console.WriteLine("Restaurant name and cuisine are required!");
            }
        }


        static void ViewAllRestaurantsAdmin()
        {
            Console.WriteLine("\n=== ALL RESTAURANTS ===");
            if (restaurants.Count == 0)
            {
                Console.WriteLine("No restaurants found!");
                return;
            }

            foreach (var restaurant in restaurants)
            {
                Console.WriteLine($"\nID: {restaurant.RestaurantId}");
                Console.WriteLine($"Name: {restaurant.Name}");
                Console.WriteLine($"Cuisine: {restaurant.CuisineType}");
                Console.WriteLine($"Phone: {restaurant.Phone}");
                Console.WriteLine($"Menu Items: {restaurant.Menu.Count}");
                Console.WriteLine("---");
            }
        }

        static void AddFoodItemToRestaurant()
        {
            ViewAllRestaurantsAdmin();
            Console.Write("\nEnter Restaurant ID to add food item: ");
            if (int.TryParse(Console.ReadLine(), out int restId))
            {
                var restaurant = restaurants.FirstOrDefault(r => r.RestaurantId == restId);
                if (restaurant != null)
                {
                    Console.Write("Enter Food Item Name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter Description: ");
                    string description = Console.ReadLine();

                    Console.Write("Enter Price: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        Console.Write("Enter Category: ");
                        string category = Console.ReadLine();

                        var newItem = new FoodItem(
                            restaurant.Menu.Count + 1,
                            restId,
                            name,
                            description,
                            price,
                            category
                        );

                        restaurant.AddFoodItem(newItem);
                        Console.WriteLine($"\n🎉 Food item '{name}' added to {restaurant.Name}!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid price!");
                    }
                }
                else
                {
                    Console.WriteLine("Restaurant not found!");
                }
            }
        }

        static void ViewAllOrders()
        {
            Console.WriteLine("\n=== ALL ORDERS ===");
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found!");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"\nOrder ID: {order.OrderId}");
                Console.WriteLine($"User ID: {order.UserId}");
                Console.WriteLine($"Date: {order.OrderDate}");
                Console.WriteLine($"Status: {order.Status}");
                Console.WriteLine($"Total: ₹{order.TotalAmount}");
                Console.WriteLine($"Payment: {order.PaymentMethod} ({order.PaymentStatus})");
                Console.WriteLine("---");
            }
        }

        //static void ViewAllUsers()
        //{
        //    Console.WriteLine("\n=== ALL USERS ===");
        //    // In real application, you would fetch from database
        //    Console.WriteLine("User management feature would connect to database here.");
        //    Console.WriteLine("This would show all registered users with their details.");
        //}


        static void ViewAllUsers()
        {
            Console.WriteLine("\n=== ALL REGISTERED USERS ===");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    //Query to get all users from database ===
                    string query = @"SELECT UserId, Name, Email, Phone, IsAdmin 
                           FROM Users";                          
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int userCount = 0;
                    while (reader.Read())
                    {
                        userCount++;
                        int userId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string email = reader.GetString(2);
                        string phone = reader.IsDBNull(3) ? "Not provided" : reader.GetString(3);
                        bool isAdmin = reader.GetBoolean(4);                       
                        Console.WriteLine($"\n USER #{userCount}");
                        Console.WriteLine($"   ID: {userId}");
                        Console.WriteLine($"   Name: {name}");
                        Console.WriteLine($"   Email: {email}");
                        Console.WriteLine($"   Phone: {phone}");
                        Console.WriteLine($"   Role: {(isAdmin ? " Admin" : " Customer")}");                       
                        Console.WriteLine($"   Status: Active");
                        Console.WriteLine("   ─────────────────────────");
                    }

                    reader.Close();

                    if (userCount == 0)
                    {
                        Console.WriteLine("No users found in the system!");
                    }
                    else
                    {
                        Console.WriteLine($"\n Total Users: {userCount}");

                        //  Get user statistics ===
                        string statsQuery = "SELECT COUNT(*) as Total, SUM(CASE WHEN IsAdmin = 1 THEN 1 ELSE 0 END) as Admins FROM Users";
                        SqlCommand statsCmd = new SqlCommand(statsQuery, conn);
                        SqlDataReader statsReader = statsCmd.ExecuteReader();

                        if (statsReader.Read())
                        {
                            int totalUsers = statsReader.GetInt32(0);
                            int adminCount = statsReader.GetInt32(1);
                            int customerCount = totalUsers - adminCount;

                            Console.WriteLine($"    Customers: {customerCount}");
                            Console.WriteLine($"    Admins: {adminCount}");
                        }
                        statsReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error fetching users: {ex.Message}");
                }
            }
        }

        static void ViewSystemStatistics()
        {
            Console.WriteLine("\n=== SYSTEM STATISTICS ===");
            Console.WriteLine($"Total Restaurants: {restaurants.Count}");
            Console.WriteLine($"Total Orders: {orders.Count}");
            Console.WriteLine($"Total Users: {users.Count + 1}"); // +1 for current user
            Console.WriteLine($"Total Revenue: ₹{orders.Sum(o => o.TotalAmount)}");

            if (orders.Count > 0)
            {
                Console.WriteLine($"Average Order Value: ₹{orders.Average(o => o.TotalAmount):F2}");
            }
        }
        static void InitializeSampleData()
        {
            // Create sample restaurants (keeping this for demo)
            var restAddress1 = new Address(2, "456 Food Street", "Mumbai", "Maharashtra", "400002", "India", "Restaurant");
            var restaurant1 = new Restaurant(1, "Spice Garden", "Indian", restAddress1, "1111111111", "info@spicegarden.com");

            // Add food items to restaurant
            restaurant1.AddFoodItem(new FoodItem(1, 1, "Butter Chicken", "Creamy butter chicken", 450, "Main Course"));
            restaurant1.AddFoodItem(new FoodItem(2, 1, "Biryani", "Hyderabadi biryani", 350, "Main Course"));
            restaurant1.AddFoodItem(new FoodItem(3, 1, "Naan", "Butter naan", 50, "Bread"));

            restaurants.Add(restaurant1);

            // Initialize cart for current user
            currentCart = new Cart(1, currUser.currentUser.UserId);
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== FOMATO MAIN MENU ===");
                Console.WriteLine("1. View Restaurants");
                Console.WriteLine("2. View Cart");
                Console.WriteLine("3. Place Order");
                Console.WriteLine("4. View My Orders");
                Console.WriteLine("5. Logout");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewRestaurants();
                        break;
                    case "2":
                        ViewCart();
                        break;
                    case "3":
                        PlaceOrder();
                        break;
                    case "4":
                        ViewOrders();
                        break;
                    case "5":
                        Console.WriteLine("Logged out successfully!");
                        currUser.currentUser = null;
                        currentCart = null;
                        ShowAuthenticationMenu();
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static void ViewRestaurants()
        {
            Console.WriteLine("\n=== AVAILABLE RESTAURANTS ===");
            foreach (var restaurant in restaurants)
            {
                Console.WriteLine($"\n{restaurant.RestaurantId}. {restaurant.Name}");
                Console.WriteLine($"   Cuisine: {restaurant.CuisineType}");
                Console.WriteLine($"   Address: {restaurant.Address.GetFullAddress()}");

                // Show menu items
                Console.WriteLine("   Menu:");
                foreach (var item in restaurant.Menu)
                {
                    Console.WriteLine($"     - {item.Name}: ₹{item.Price}");
                }
            }

            Console.Write("\nEnter Restaurant ID to order (0 to go back): ");
            if (int.TryParse(Console.ReadLine(), out int restId) && restId != 0)
            {
                var selectedRestaurant = restaurants.FirstOrDefault(r => r.RestaurantId == restId);
                if (selectedRestaurant != null)
                {
                    AddToCart(selectedRestaurant);
                }
                else
                {
                    Console.WriteLine("Invalid Restaurant ID!");
                }
            }
        }

        static void AddToCart(Restaurant restaurant)
        {
            while (true)
            {
                Console.WriteLine($"\n=== {restaurant.Name} MENU ===");
                foreach (var item in restaurant.Menu)
                {
                    Console.WriteLine($"{item.ItemId}. {item.Name} - ₹{item.Price}");
                }

                Console.Write("Enter Item ID to add to cart (0 to finish): ");
                if (int.TryParse(Console.ReadLine(), out int itemId))
                {
                    if (itemId == 0) break;

                    var selectedItem = restaurant.Menu.FirstOrDefault(item => item.ItemId == itemId);
                    if (selectedItem != null)
                    {
                        Console.Write($"Enter quantity for {selectedItem.Name}: ");
                        if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                        {
                            currentCart.AddToCart(selectedItem, quantity);
                        }
                        else
                        {
                            currentCart.AddToCart(selectedItem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Item ID!");
                    }
                }
            }
        }

        static void ViewCart()
        {
            currentCart.DisplayCart();
        }

        static void PlaceOrder()
        {
            if (currentCart.Items.Count == 0)
            {
                Console.WriteLine("Your cart is empty! Add some items first.");
                return;
            }

            Console.WriteLine("\n=== ORDER SUMMARY ===");
            currentCart.DisplayCart();

            Console.Write("\nConfirm order? (y/n): ");
            string confirm = Console.ReadLine().ToLower();

            if (confirm == "y")
            {
                // Create new order
                int newOrderId = orders.Count + 1;
                var order = new Order(newOrderId, currUser.currentUser.UserId, currentCart.Items.First().RestaurantId, 1);

                // Add all cart items to order
                foreach (var cartItem in currentCart.Items)
                {
                    var orderItem = new OrderItem(
                        orderItemId: order.OrderItems.Count + 1,
                        orderId: order.OrderId,
                        foodItemId: cartItem.FoodItemId,
                        foodItemName: cartItem.FoodItemName,
                        quantity: cartItem.Quantity,
                        unitPrice: cartItem.UnitPrice
                    );
                    order.AddOrderItem(orderItem);
                }

                order.Status = "Confirmed";
                order.PaymentStatus = "Completed";
                orders.Add(order);

                Console.WriteLine("\n🎉 ORDER PLACED SUCCESSFULLY!");
                Console.WriteLine($"Order ID: {order.OrderId}");
                Console.WriteLine($"Total Amount: ₹{order.TotalAmount}");
                Console.WriteLine($"Status: {order.Status}");

                // Clear cart after order
                currentCart.ClearCart();
            }
            else
            {
                Console.WriteLine("Order cancelled.");
            }
        }

        static void ViewOrders()
        {
            var userOrders = orders.Where(o => o.UserId == currUser.currentUser.UserId).ToList();

            if (userOrders.Count == 0)
            {
                Console.WriteLine("No orders found!");
                return;
            }

            Console.WriteLine("\n=== YOUR ORDERS ===");
            foreach (var order in userOrders)
            {
                Console.WriteLine($"\nOrder ID: {order.OrderId}");
                Console.WriteLine($"Date: {order.OrderDate}");
                Console.WriteLine($"Status: {order.Status}");
                Console.WriteLine($"Total: ₹{order.TotalAmount}");
                Console.WriteLine("Items:");
                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine($"  - {item.FoodItemName} x {item.Quantity} = ₹{item.TotalPrice}");
                }
                Console.WriteLine("---");
            }
        }
    }
}