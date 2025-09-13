using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class FoodItemDto
    {
        public int restaurant_id { get; set; }

        public string item_name { get; set; }

        public decimal price { get; set; }

        public decimal rating { get; set; }

        public int category_id { get; set; }

        public bool status { get; set; }

        public string Description { get; set; }

        public string keywords { get; set; }
    }
}
