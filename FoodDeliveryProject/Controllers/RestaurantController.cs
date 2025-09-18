using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.Repositories;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurant restaurant;
        private readonly IFoodItems foodItems;

        public RestaurantController(IRestaurant restaurant,IFoodItems foodItems)
        {
            this.restaurant=restaurant;
            this.foodItems = foodItems;
        }


        [HttpGet]
        public IActionResult GetRestaurants()
        {
            var restaurants=restaurant.GetRestaurants();
            if(restaurants == null)
            {
                return NotFound();
            }
            return Ok(restaurants);
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult<RestaurantDto> GetRestaurantById([FromRoute] int id)
        { 
            var restaurantWithThatId= restaurant.GetRestaurantById(id);
            if(restaurantWithThatId == null)
            {
                return NotFound();
            }
            return Ok(restaurantWithThatId);

        }
        [Authorize(Roles = "restaurant")]
        [HttpPost]
        public IActionResult AddRestaurant([FromBody] RestaurantCreateDto restaurantCreateDto)
        {
            var created = restaurant.AddRestaurant(restaurantCreateDto);
            if (created == null)
            {
                return BadRequest("");
            }
            return Ok(created);


        }
        [Authorize(Roles = "restaurant")]
        [HttpPut]
        [Route("{id}/{status}")]
        public IActionResult UpdateRestaurantStatus([FromRoute] int id, [FromRoute] bool status)
        {
            var updated=restaurant.UpdateRestaurantStatus(id, status);
            if (!updated)
            {
                return NotFound(new { message = "Restaurant not found with that id" });

            }
            return Ok(new {message="Restaurant status updated successfully"});
        }
        [Authorize(Roles = "restaurant,admin")]
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteRestaurant(int id)
        {
            var deleted=restaurant.DeleteRestaurant(id);
            if (!deleted)
            {
                return NotFound(new {message="restaurant not found with that id"});
            }
            return Ok(new { message = "Restaurant deleted successfully" });
        }
        [Authorize(Roles = "restaurant")]
        [HttpPost("addfooditems")]
        public IActionResult AddFoodItems([FromQuery] FoodItemDto foodItemCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdFoodItem = foodItems.CreateFoodItem(foodItemCreateDto);
            return Ok(createdFoodItem);
        }
        //update food items
        //this for there may be price change etc..
        [Authorize(Roles = "restaurant")]
        [HttpPut("update/fooditem")]
        public IActionResult UpdateFoodItem([FromQuery] int foodid, [FromQuery] FoodItemDto fooddto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("FoodItem Updation Cannot be done");
            }
            var updatedfood = foodItems.UpdateFoodItem(foodid, fooddto);
            return Ok(updatedfood);
        }
        [Authorize(Roles = "restaurant")]
        [HttpDelete("delete/fooditem/{itemid}")]
        public IActionResult DeleteFoodItem(int itemid)
        {
            var item =foodItems.DeleteFoodItemByFoodId(itemid);
            if (item)
            {
                return Ok("Food Item Successfully deleted");
            }
            return NotFound("Cannot find the item in Database");
        }
        [Authorize(Roles = "restaurant,admin,customer")]
        [HttpGet]
        [Route("RestaurantWithFoodItems")]
        public IActionResult RestaurantWithThereFoodItems()
        {
            var result = restaurant.RestaurantWithThereFoodItems();
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "no restaurants found" });
            }
            return Ok(result);
        }
        //[HttpGet]
        //[Route("byrole/{role}")]
        //public IActionResult GetRestaurantByRole([FromRoute] string role)
        //{
        //    var restaurantWithThatRole = restaurant.GetRestaurantByRole(role);
        //    if (restaurantWithThatRole == null)
        //    {
        //        return NotFound(new { message = "Not restaurants" });
        //    }
        //    return Ok(restaurantWithThatRole);
        //}
        [Authorize(Roles = "admin")]
        [HttpGet("open-restaurants")]
        public ActionResult<List<RestaurantDto>> GetOpenRestaurants()
        {
            var restaurants=restaurant.GetOpenRestaurants();
            if (restaurants == null || restaurants.Count == 0)
            {
                return NotFound("No open Restaurants available");
            }
            return Ok(restaurants);
        }
    }
}
