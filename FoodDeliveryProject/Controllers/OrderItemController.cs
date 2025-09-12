using Domain.DTO;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItem _fooditems;

        public OrderItemController(IOrderItem fooditems)
        {
            _fooditems = fooditems;
        }

        [HttpGet("getfooditemsbyid/{id}")]
        public IActionResult GetFoodByOrder(int id)
        {
            IEnumerable<OrderItemDto> foodItems = _fooditems.GetFoodByOrderId(id);
            return Ok(foodItems);
        }
    }
}
