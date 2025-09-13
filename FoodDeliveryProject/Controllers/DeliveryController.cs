using Domain.Models; 
using Domain.ADO;    
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.DTO;

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
        [HttpGet("DeliveryDetailsByOrderId/{orderId}")]
        public ActionResult<DeliveryDto> GetDeliveryDetailsByOrderId(int orderId, string CustAddress)
        {
            var delivery = _deliveryService.GetDeliveryDetailsByOrderId(orderId, CustAddress);
            if (delivery == null)
            {
                return NotFound($"No delivery found for Order ID: {orderId}");
            }
            return Ok(delivery);
        }

       

    }
}
