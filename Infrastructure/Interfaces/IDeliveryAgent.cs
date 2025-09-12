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
        List<DeliveryDto> GetDeliveriesByAgentId(int AgentId);
        bool UpdateDeliveryStatus(int DeliveryId);

    }
}
