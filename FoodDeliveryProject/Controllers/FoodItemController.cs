using Domain.DTO;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize(Roles = "Admin,customer")]
        //[HttpGet("GetFoodItemsByKeywords")]
        //public IActionResult GetByFoodItems(string keyword)
        //{
        //    var foodItems = _foodItemsService.GetFoodItemsByKeywords(keyword);
        //    if (foodItems == null)
        //    {
        //        return NotFound("No food items by the keyword");
        //    }
        //    return Ok(foodItems);
        //}


        ////add food items


        //[Authorize(Roles = "restaurant")]
        //[HttpPost("addfooditems")]
        //public IActionResult AddFoodItems([FromQuery] FoodItemDto foodItemCreateDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var createdFoodItem = _foodItemsService.CreateFoodItem(foodItemCreateDto);
        //    return Ok(createdFoodItem);
        //}


        ////update food items

        //[Authorize(Roles = "restaurant")]
        //[HttpPut("update/fooditem")]
        //public IActionResult UpdateFoodItem([FromQuery] int foodid, [FromQuery] FoodItemDto fooddto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return NotFound("FoodItem Updation Cannot be done");
        //    }
        //    var updatedfood = _foodItemsService.UpdateFoodItem(foodid, fooddto);
        //    return Ok(updatedfood);
        //}


        ////delete food items


        //[Authorize(Roles = "restaurant")]
        //[HttpDelete("delete/fooditem/{itemid}")]
        //public IActionResult DeleteFoodItem(int itemid)
        //{
        //    var item = _foodItemsService.DeleteFoodItemByFoodId(itemid);
        //    if (item)
        //    {
        //        return Ok("Food Item Successfully deleted");
        //    }
        //    return NotFound("Cannot find the item in Database");
        //}
        //[Authorize(Roles = "customer,admin")]
        //[HttpGet("get/foodItemsByRestaurant/{name}")]
        //public IActionResult GetFoodItemsByRestaurant([FromRoute] string name)
        //{
        //    var foodItemsByRestaurant = _foodItemsService.GetFoodItemsByRestaurant(name);
        //    return Ok(foodItemsByRestaurant);
        //}
        [HttpGet("get/fooditembyid")]
        public IActionResult GetFoodItems([FromQuery] int foodid)
        {
            var food = _foodItemsService.GetFoodItemById(foodid);
            return Ok(food);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchFoodItems([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Keyword is required.");

            var results = await _foodItemsService.SearchFoodItemsAsync(keyword);
            return Ok(results);
        }
        [HttpGet("ByCategory")]
        public IActionResult GetFoodItemsByCategory([FromQuery] string category)
        {
            var items = _foodItemsService.GetFoodItemsByCategory(category);
            if (items == null || items.Count == 0)
                return NotFound("No food items found for this category.");

            return Ok(items);
        }
    }
}
