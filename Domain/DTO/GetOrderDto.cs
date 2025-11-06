using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryProject.DTOs
{
    public class FoodItemDtoWithPrice
    {
        public string ItemName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalItemPrice { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class GetOrderDto
    {
        public int id { get; set; }
        public string RestaurantName { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public List<FoodItemDtoWithPrice> FoodItems { get; set; } = new();
        public string Status{ get; set; } 
        public int AgentId { get; set; }
        public string AgentName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}

