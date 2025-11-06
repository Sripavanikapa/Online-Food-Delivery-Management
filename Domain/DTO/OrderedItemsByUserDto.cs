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

        [Column("TotalPrice")]

        public decimal TotalPrice { get; set; }

        [Column("rating")]

        public decimal Rating { get; set; }

        [Column("imageurl")]

        public string imageurl { get; set; }

        [Column("quantity")]

        public int quantity { get; set; }

        [Column("status")]

        public string Status { get; set; }

        [Column("CreatedAt")]

        public DateTime Date { get; set; }


    }

}

