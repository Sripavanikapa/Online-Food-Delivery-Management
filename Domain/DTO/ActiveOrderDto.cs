using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ActiveOrderDto
    {
       
        
            public int OrderId { get; set; }
            public string Status { get; set; }
            public string RestaurantName { get; set; }
            public string RestaurantAddress { get; set; }
            public string CustomerName { get; set; }
            public string CustomerAddress { get; set; }
            public decimal TotalPrice { get; set; }
        }
    
}

