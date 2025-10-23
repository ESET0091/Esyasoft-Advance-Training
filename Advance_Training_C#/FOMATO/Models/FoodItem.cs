using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOMATO.Models
{
    public class FoodItem
    {
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
        public int PreparationTime { get; set; } // in minutes

        public FoodItem(int itemId, int restaurantId, string name, string description, decimal price, string category)
        {
            ItemId = itemId;
            RestaurantId = restaurantId;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            IsAvailable = true;
            PreparationTime = 20;
        }

        public void DisplayItem()
        {
            Console.WriteLine($"{ItemId}. {Name} - ₹{Price}");
            Console.WriteLine($"   {Description}");
            Console.WriteLine($"   Category: {Category}");
            Console.WriteLine($"   Preparation Time: {PreparationTime} mins");
            Console.WriteLine($"   Available: {(IsAvailable ? "Yes" : "No")}");
        }
    }
}