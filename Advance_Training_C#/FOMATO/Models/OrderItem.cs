using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOMATO.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderItem(int orderItemId, int orderId, int foodItemId, string foodItemName, int quantity, decimal unitPrice)
        {
            OrderItemId = orderItemId;
            OrderId = orderId;
            FoodItemId = foodItemId;
            FoodItemName = foodItemName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = quantity * unitPrice;
        }

        public void DisplayOrderItem()
        {
            Console.WriteLine($"  - {FoodItemName} x {Quantity} = ₹{TotalPrice}");
        }
    }
}