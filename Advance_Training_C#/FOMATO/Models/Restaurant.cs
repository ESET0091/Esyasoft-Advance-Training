using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOMATO.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string CuisineType { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Rating { get; set; }
        public List<FoodItem> Menu { get; set; }

        public Restaurant(int restaurantId, string name, string cuisineType, Address address, string phone, string email)
        {
            RestaurantId = restaurantId;
            Name = name;
            CuisineType = cuisineType;
            Address = address;
            Phone = phone;
            Email = email;
            Rating = 0.0m;
            Menu = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem item)
        {
            Menu.Add(item);
        }

        public void DisplayRestaurantInfo()
        {
            Console.WriteLine($"\nRestaurant ID: {RestaurantId}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Cuisine: {CuisineType}");
            Console.WriteLine($"Address: {Address.GetFullAddress()}");
            Console.WriteLine($"Phone: {Phone}");
            Console.WriteLine($"Rating: {Rating}/5");
            Console.WriteLine("Menu:");
            foreach (var item in Menu)
            {
                item.DisplayItem();
            }
        }
    }
}