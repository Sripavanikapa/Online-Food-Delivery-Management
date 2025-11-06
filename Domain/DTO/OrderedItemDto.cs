using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderedItemDto
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalItemPrice => Price * Quantity;
    }
}
