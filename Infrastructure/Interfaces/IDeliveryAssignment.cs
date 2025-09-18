using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IDeliveryAssignment
    {
        Task<Delivery> AssignOrderAsync(Order order,Restaurant restaurant,User customer,List<DeliveryAgent> AvailabledeliveryAgents);
    }
}
