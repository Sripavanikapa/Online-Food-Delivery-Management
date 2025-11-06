using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AddOrderDto
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public int CustAddressId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AddOrderItemDto> Items { get; set; } = new();
    }

}
