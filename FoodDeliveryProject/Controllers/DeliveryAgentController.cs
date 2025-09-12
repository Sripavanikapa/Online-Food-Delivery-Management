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
