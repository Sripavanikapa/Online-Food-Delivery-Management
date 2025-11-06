using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AddOrderItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
