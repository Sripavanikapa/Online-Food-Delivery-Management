using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class FoodItemDtoForRestaurant
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }

        public  int CategoryId { get; set; }
        public string Description { get; set; }

        public string Keywords { get; set; }

        public bool Status { get; set; }
        public string ImageUrl { get; set; }
    }
}
