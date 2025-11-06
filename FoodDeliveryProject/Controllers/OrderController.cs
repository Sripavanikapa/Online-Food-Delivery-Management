using Domain.DTO;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _order;

        public OrderController(IOrder order)
        {
            _order = order;
        }

        // Add a new order

        [Authorize(Roles = "customer")]
        [HttpPost("AddingordersbyId")]
        public IActionResult AddingOrder([FromBody] AddOrderDto dto)
        {
            if (dto.UserId <= 0 || dto.RestaurantId <= 0)
            {
                return BadRequest("Invalid order data.");
            }

            _order.AddOrder(dto);
            return Ok("Order added successfully.");
        }


        // Get orders by order id

        [Authorize(Roles="admin,restaurant")]
        [HttpGet("GetOrderbyorderId/{id}")]
        public IActionResult GetOrderByOrderId(int id)
        {
            var order = _order.GetOrderByOrderId(id);

            if (order == null)
                return NotFound("Order not found.");

            return Ok(order);
        }

        [HttpGet("GetActiveOrders")]
        public IActionResult GetActiveOrders(int id)
        {
            var orders = _order.GetActiveOrdersByAgentId(id);
            if (orders == null || !orders.Any())
                return NotFound("No active orders found.");

            return Ok(orders);
        }
        [HttpPut("UpdateStatusToDelivered")]
        public IActionResult UpdateStatusToDelivered(int orderId)
        {
            var updated = _order.UpdateOrderStatusToDelivered(orderId);
            if (updated)
                return Ok("Order status updated to Delivered.");
            else
                return NotFound("Order not found or already delivered.");
        }
    }
}
