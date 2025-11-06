using Domain.Data;
using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.Repositories;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurant restaurant;
        private readonly IFoodItems foodItems;
        private readonly IOrder order;

        public RestaurantController(IRestaurant restaurant,IFoodItems foodItems,IOrder order)
        {
            this.restaurant=restaurant;
            this.foodItems = foodItems;
            this.order = order;
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
        [Route("{id}/status")]
        public IActionResult UpdateRestaurantStatus([FromRoute] int id, [FromBody] RestaurantStatusUpdateDto restaurantStatusUpdateDto)
        {
            var updated=restaurant.UpdateRestaurantStatus(id, restaurantStatusUpdateDto.Status);
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
        public IActionResult UpdateFoodItem([FromForm] int foodid, [FromForm] FoodItemDto fooddto)
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
        [AllowAnonymous]
        [HttpGet("restaurant/{restaurantId}/incoming")]
        //[Authorize(Roles = "restaurant")]
        public IActionResult GetIncomingOrders(int restaurantId)
        {
            var orders = order.GetIncomingOrdersForRestaurant(restaurantId);
            if (orders == null || !orders.Any())
            {
                return NotFound("No incoming orders found for this restaurant.");
            }
            return Ok(orders);
        }
        [AllowAnonymous]
        [HttpPut("order/{orderId}/status")]
        public IActionResult UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
            {
                return BadRequest("Status is required.");
            }

            var success = order.UpdateOrderStatus(orderId, dto.Status);
            if (!success)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            return Ok(new { message = $"Order {orderId} marked as {dto.Status}." });
        }
        [Authorize(Roles = "restaurant")]
        [HttpPut("update-fooditem")]
        public async Task<IActionResult> UpdateFoodItemWithImage([FromForm] int foodid, [FromForm] FoodItemDto dto, IFormFile image)
        {
            try
            {
                var updated = await foodItems.UpdateFoodItemWithImageAsync(foodid, dto, image);
                if (updated == null)
                    return NotFound("Food item not found.");

                return Ok(new { message = "Food item updated successfully", data = updated });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "restaurant,deliveryagent")]
        [HttpGet("me")]
        public IActionResult GetMyRestaurant()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var restaurantinfo = restaurant.GetRestaurantByUserId(userId);
            if (restaurantinfo == null)
            {
                return NotFound("Restaurant not found for this user.");
            }
            return Ok(restaurantinfo);
        }
        [HttpPost("upload-fooditem")]
        public async Task<IActionResult> UploadFoodItem([FromForm] FoodItemDto dto, IFormFile image)
        {
            var result = await foodItems.UploadFoodItemAsync(dto, image);
            return Ok(result);
        }

        [Authorize(Roles = "restaurant")]
        [HttpGet("order-history/{restaurantId}")]
        public IActionResult GetOrderHistory(int restaurantId)
        {
            var history = restaurant.GetOrderHistoryForRestaurant(restaurantId);
            if (history == null || !history.Any())
                return NotFound("No order history found for this restaurant.");

            return Ok(history);
        }
        [Authorize(Roles = "restaurant")]
        [HttpPost("create-fooditem")]
        public IActionResult CreateFoodItem([FromBody] FoodItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = foodItems.CreateFoodItem(dto);
            if (result == null)
            {
                return BadRequest("Failed to create food item.");
            }

            return Ok(result);
        }


    }
}
