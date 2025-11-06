using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.Repositories;
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

        [Authorize(Roles = "deliveryagent")]
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

        [Authorize(Roles = "deliveryagent")]
        [HttpGet("updateDeliveryStatus")]
        public ActionResult<string> UpdateDeliveryStatus([FromQuery] int DeliveryId)
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

        [HttpGet("GetAgentName")]

        public ActionResult<string> GetAgentName(int id)
        {


            var name = _deliveryAgentService.GetAgentName(id);
            if (string.IsNullOrEmpty(name))
            {
                return NotFound("Agent not found.");
            }
            return Ok(name);


        }


        [HttpGet("GetAgentPhone")]
        public IActionResult GetAgentPhone(int id)
        {
            var phone = _deliveryAgentService.GetAgentPhone(id);
            if (phone == null)
            {
                return NotFound("Phone number not found for the agent.");
            }
            return Ok(phone);
        }


        [Authorize(Roles = "deliveryagent")]
        [HttpGet("updateAgentStatus")]
        public ActionResult<string> UpdateAgentStatus([FromQuery] int AgentId, bool status)
        {

            var result = _deliveryAgentService.UpdateAgentStatus(AgentId, status);
            if (!result)
            {
                return NotFound("Agent not found.");
            }

            return Ok($"Agent status{(status ? " Updated to Available" : " Updated to Not Available")}.");

        }
        [Authorize(Roles = "deliveryagent")]
        [HttpGet("Agentorder-history/{agentId}")]
        public IActionResult GetOrderHistory(int agentId)
        {
            var history = _deliveryAgentService.GetOrderHistoryForAgent(agentId);
            if (history == null || !history.Any())
                return NotFound("No order history found for this agent.");

            return Ok(history);
        }
    }
}
