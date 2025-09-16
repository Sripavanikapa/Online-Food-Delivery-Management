using Domain.ADO;    
using Domain.DTO;
using Domain.Models; 
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryServices _deliveryService;

        public DeliveryController()
        {
            _deliveryService = new DeliveryServices();
        }

        //for delivery agent

        [Authorize(Roles = "DeliveryAgent")]
        [HttpGet("DeliveryDetailsByOrderId/{orderId}")]
        public ActionResult<DeliveryDto> GetDeliveryDetailsByOrderId(int orderId)
        {
            var delivery = _deliveryService.GetDeliveryDetailsByOrderId(orderId);
            if (delivery == null)
            {
                return NotFound($"No delivery found for Order ID: {orderId}");
            }
            return Ok(delivery);
        }

       

    }
}
