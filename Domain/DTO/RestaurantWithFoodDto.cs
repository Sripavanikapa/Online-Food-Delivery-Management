using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class RestaurantWithFoodDto
    {

        public int RestaurantId { get; set; }
        public bool Status { get; set; }

        public string? OwnerName { get; set; }

        public virtual object  FoodItems { get; set; }
        
    }
}
