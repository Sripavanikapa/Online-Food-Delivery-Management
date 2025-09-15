using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class DeliveryETARequest
    {
        public string CustomerAddress { get; set; }
        public string AgentAddress { get; set; }
    }
}
