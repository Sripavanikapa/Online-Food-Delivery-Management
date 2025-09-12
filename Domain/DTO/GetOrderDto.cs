using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryProject.DTOs
{
    public class FoodItemDto
    {
        public string ItemName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalItemPrice { get; set; }
    }

    public class GetOrderDto
    {
        public string RestaurantName { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public List<FoodItemDto> FoodItems { get; set; } = new();
    }
}

