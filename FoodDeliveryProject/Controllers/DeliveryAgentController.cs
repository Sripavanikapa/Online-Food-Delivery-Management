using Domain.DTO;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


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



        // get all deliveries assigned to and done by a delivery agent by agent phone no

        [Authorize(Roles = "DeliveryAgent")]
        [HttpGet("AllDeliveriesByAgent")]
        public ActionResult<List<AllDeliveryDto>> GetDeliveriesByAgentPhone(string phone)
        {
            var deliveries = _deliveryAgentService.GetDeliveriesByAgentPhone(phone);
            if (deliveries == null)
            {
                return NotFound("No deliveries found.");
            }
            return deliveries;
        }

        [Authorize(Roles = "DeliveryAgent")]
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

        //for assigning agents
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
