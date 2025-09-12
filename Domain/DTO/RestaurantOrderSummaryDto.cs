using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class RestaurantOrderSummaryDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public int NoOfOrders { get; set; }
    }
}
