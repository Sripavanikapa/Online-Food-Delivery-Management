using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderedItemsByUserDto
    {
        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("name")]
        public string RestaurantName { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("rating")]
        public decimal Rating { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
