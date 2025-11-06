using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderHistoryDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderedItemDto> Items { get; set; }
    }
}
