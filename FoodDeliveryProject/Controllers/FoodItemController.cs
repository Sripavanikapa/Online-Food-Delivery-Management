using Domain.DTO;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItems _foodItemsService;
        public FoodItemController(IFoodItems fooditems)
        {
            _foodItemsService = fooditems;
        }

        //get food items by keywords

        [HttpGet("GetFoodItemsByKeywords")]
        public IActionResult GetByFoodItems(string keyword)
        {
            var foodItems = _foodItemsService.GetFoodItemsByKeywords(keyword);
            if (foodItems == null)
            {
                return NotFound("No food items by the keyword");
            }
            return Ok(foodItems);
        }

        [HttpPost("addfooditems")]
        public IActionResult AddFoodItems([FromQuery] FoodItemDto foodItemCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdFoodItem = _foodItemsService.CreateFoodItem(foodItemCreateDto);
            return Ok(createdFoodItem);
        }

        [HttpPut("update/fooditem")]
        public IActionResult UpdateFoodItem([FromQuery] int foodid, [FromQuery] FoodItemDto fooddto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("FoodItem Updation Cannot be done");
            }
            var updatedfood = _foodItemsService.UpdateFoodItem(foodid,fooddto);
            return Ok(updatedfood);
        }

        [HttpDelete("delete/fooditem/{itemid}")]
        public IActionResult DeleteFoodItem(int itemid)
        {
            var item = _foodItemsService.DeleteFoodItemByFoodId(itemid);
            if (item)
            {
                return Ok("Food Item Successfully deleted");
            }
            return NotFound("Cannot find the item in Database");
        }
    }
}
