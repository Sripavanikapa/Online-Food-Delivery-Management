using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class FoodItemDto
    {
        public int ItemId { get; set; }
        public int restaurant_id { get; set; }

        [Column("item_name")]
        public string itemName { get; set; }


        public decimal price { get; set; }

        public int category_id { get; set; }

        public bool status { get; set; }

        public string Description { get; set; }
        public string CategoryName { get; set; }

        public string keywords { get; set; }

        [Column("imageurl")]
        public string imageurl { get; set; }
        public int Quantity { get; set; }
    }
}
