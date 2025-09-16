using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.DTO;

namespace Infrastructure.Interfaces
{
    public interface IDeliveryAgent
    {
        List<AllDeliveryDto> GetDeliveriesByAgentPhone(string agentPhoneNo);
        bool UpdateDeliveryStatus(int DeliveryId);
        List<DeliveryAgentDto> GetAvailableAgents();
    }
}
