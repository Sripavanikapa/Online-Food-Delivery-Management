using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AllDeliveryDto
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public bool Status { get; set; }

    }
}
