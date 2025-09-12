using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class DeliveryDto
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
       // public int AgentId { get; set; }
        public string restaurantName { get; set; }
        public string restaurantAddress { get; set; }
        public string customerName { get; set; }
        public string customerAddress { get; set; }
        //public int Price{ get; set; }
        public bool Status { get; set; }
    }
}
