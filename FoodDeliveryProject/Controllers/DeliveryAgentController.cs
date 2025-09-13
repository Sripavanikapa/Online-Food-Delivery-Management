using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.DTO;





namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAgentController : ControllerBase
    {
        private readonly DeliveryAgentService _deliveryAgentService;
        public DeliveryAgentController()
        {
            _deliveryAgentService = new DeliveryAgentService();
        }

        [HttpGet("AllDeliveriesByAgent")]
        public ActionResult<List<DeliveryDto>> GetDeliveriesByAgentId(int AgentId)
        {
            var deliveries = _deliveryAgentService.GetDeliveriesByAgentId(AgentId);
            if (deliveries == null)
            {
                return NotFound("No deliveries found.");
            }
            return deliveries;
        }

        [HttpGet("updateDeliveryStatus")]
        public ActionResult<string> UpdateDeliveryStatus(int DeliveryId)
        {
            var status = _deliveryAgentService.UpdateDeliveryStatus(DeliveryId);
            if (!status)
            {
                return ("Delivery Pending");
            }
            return "Delivered";
        }


        [HttpGet("AvailableAgents")]
        public ActionResult<List<DeliveryAgentDto>> GetAvailableAgents()
        {
            var agents = _deliveryAgentService.GetAvailableAgents();
            if (agents == null)
            {
                return NotFound("No delivery agents found.");
            }
            return agents;
        }
    }
}

