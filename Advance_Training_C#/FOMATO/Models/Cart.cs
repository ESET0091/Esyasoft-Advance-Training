using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;

namespace FOMATO.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Cart(int cartId, int userId)
        {
            CartId = cartId;
            UserId = userId;
            Items = new List<CartItem>();
            TotalAmount = 0;
            TotalItems = 0;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        // Constructor for loading from database
        public Cart(int cartId, int userId, string cartDataJson, decimal totalAmount, int totalItems, DateTime createdAt, DateTime updatedAt)
        {
            CartId = cartId;
            UserId = userId;
            TotalAmount = totalAmount;
            TotalItems = totalItems;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;

            // Deserialize JSON string to List<CartItem>
            if (!string.IsNullOrEmpty(cartDataJson) && cartDataJson != "[]")
            {
                Items = JsonSerializer.Deserialize<List<CartItem>>(cartDataJson) ?? new List<CartItem>();
            }
            else
            {
                Items = new List<CartItem>();
            }
        }

        // Convert cart items to JSON for database storage
        public string ToJson()
        {
            return JsonSerializer.Serialize(Items);
        }

        public void AddToCart(FoodItem item, int quantity = 1)
        {
            var existingItem = Items.FirstOrDefault(i => i.FoodItemId == item.ItemId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                Items.Add(new CartItem
                {
                    FoodItemId = item.ItemId,
                    FoodItemName = item.Name,
                    Quantity = quantity,
                    UnitPrice = item.Price,
                    TotalPrice = quantity * item.Price,
                    RestaurantId = item.RestaurantId
                });
            }
            CalculateTotals();
            UpdatedAt = DateTime.Now;
            Console.WriteLine($"{item.Name} added to cart!");
        }

        public void RemoveFromCart(int foodItemId, int quantity = 1)
        {
            var existingItem = Items.FirstOrDefault(i => i.FoodItemId == foodItemId);
            if (existingItem != null)
            {
                if (existingItem.Quantity <= quantity)
                {
                    Items.Remove(existingItem);
                }
                else
                {
                    existingItem.Quantity -= quantity;
                    existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
                }
                CalculateTotals();
                UpdatedAt = DateTime.Now;
                Console.WriteLine($"Item removed from cart!");
            }
        }

        public void UpdateQuantity(int foodItemId, int newQuantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.FoodItemId == foodItemId);
            if (existingItem != null && newQuantity > 0)
            {
                existingItem.Quantity = newQuantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
                CalculateTotals();
                UpdatedAt = DateTime.Now;
            }
        }

        private void CalculateTotals()
        {
            TotalAmount = Items.Sum(item => item.TotalPrice);
            TotalItems = Items.Sum(item => item.Quantity);
        }

        public void DisplayCart()
        {
            Console.WriteLine("\n=== Your Cart ===");
            if (Items.Count == 0)
            {
                Console.WriteLine("Cart is empty!");
                return;
            }

            foreach (var item in Items)
            {
                Console.WriteLine($"  - {item.FoodItemName} x {item.Quantity} = ₹{item.TotalPrice} (₹{item.UnitPrice} each)");
            }
            Console.WriteLine($"Total Items: {TotalItems}");
            Console.WriteLine($"Total Amount: ₹{TotalAmount}");
        }

        public void ClearCart()
        {
            Items.Clear();
            TotalAmount = 0;
            TotalItems = 0;
            UpdatedAt = DateTime.Now;
            Console.WriteLine("Cart cleared!");
        }

        public bool IsEmpty()
        {
            return Items.Count == 0;
        }

        public int GetItemCount()
        {
            return Items.Count;
        }
    }

    public class CartItem
    {
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int RestaurantId { get; set; }
    }
}