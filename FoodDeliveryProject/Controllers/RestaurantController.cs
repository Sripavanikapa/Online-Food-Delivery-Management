using Domain.DTO;


using FoodDeliveryProject.Repositories;
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
        public RestaurantController(IRestaurant restaurant)
        {
            this.restaurant=restaurant;
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



    }
}
