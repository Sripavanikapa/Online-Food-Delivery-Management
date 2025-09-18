using Domain.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAgentAssignmentController : ControllerBase
    {
        private readonly IDeliveryAssignment deliveryAssignment;
        private readonly AppDbContext appDbContext;

        public DeliveryAgentAssignmentController(IDeliveryAssignment deliveryAssignment,AppDbContext appDbContext)
        {
            this.deliveryAssignment = deliveryAssignment;
            this.appDbContext = appDbContext;
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AssignDeliveryAgent([FromBody] int orderId)
        {
            var order = await appDbContext.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound("Order not found");
            var customer = await appDbContext.Users.FindAsync(order.UserId);
            var restaurant = await appDbContext.Restaurants.FindAsync(order.RestaurantId);
            var availableAgents = await appDbContext.DeliveryAgents
                                    .Where(a => a.Status == true)
                                    .Include(a => a.Agent)
                                    .ThenInclude(u => u.Addresses)
                                    .ToListAsync();
            var assignedDelivery = await deliveryAssignment.AssignOrderAsync(order, restaurant, customer, availableAgents);
            return Ok(new
            {
                Message = "Delivery agent assigned successfully",
                AgentId = assignedDelivery.AgentId,
                OrderId = assignedDelivery.OrderId,
                DeliveryId = assignedDelivery.DeliveryId
            });

        }
        }
}
