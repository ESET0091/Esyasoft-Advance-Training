using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOMATO.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Preparing, Out for Delivery, Delivered, Cancelled
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int DeliveryAddressId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; } // Pending, Completed, Failed

        public Order(int orderId, int userId, int restaurantId, int deliveryAddressId)
        {
            OrderId = orderId;
            UserId = userId;
            RestaurantId = restaurantId;
            DeliveryAddressId = deliveryAddressId;
            OrderItems = new List<OrderItem>();
            TotalAmount = 0;
            Status = "Pending";
            OrderDate = DateTime.Now;
            PaymentMethod = "Cash";
            PaymentStatus = "Pending";
        }

        public void AddOrderItem(OrderItem item)
        {
            OrderItems.Add(item);
            TotalAmount += item.TotalPrice;
        }

        public void DisplayOrder()
        {
            Console.WriteLine($"\nOrder ID: {OrderId}");
            Console.WriteLine($"Order Date: {OrderDate}");
            Console.WriteLine($"Status: {Status}");
            Console.WriteLine($"Payment Method: {PaymentMethod}");
            Console.WriteLine($"Payment Status: {PaymentStatus}");
            Console.WriteLine("Items:");
            foreach (var item in OrderItems)
            {
                item.DisplayOrderItem();
            }
            Console.WriteLine($"Total Amount: ₹{TotalAmount}");
        }
    }
}