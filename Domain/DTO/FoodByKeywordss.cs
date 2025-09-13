using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class FoodByKeywords
    {

        [Column("item_name")]

        public string ItemName { get; set; }


        [Column("name")]

        public string RestaurantName { get; set; }


        [Column("price")]

        public decimal Price { get; set; }


        [Column("Description")]

        public string Description { get; set; }
    }
}
