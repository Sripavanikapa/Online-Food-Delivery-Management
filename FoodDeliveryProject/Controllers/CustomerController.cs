using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;
using FoodDeliveryProject.Repositories;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IFoodItems foodItemsService;
        private readonly IUserRepository userService;
        private readonly IRestaurant restaurant;
        private readonly IUserRepository userServices;
        private readonly IOrder order;
        private readonly IReview review;

        public CustomerController(IFoodItems foodItemsService,IUserRepository userService,IRestaurant restaurant,IUserRepository userServices,IOrder order,IReview review)
        {
            this.foodItemsService = foodItemsService;
            this.userService = userService;
            this.restaurant = restaurant;
            this.userServices = userServices;
            this.order = order;
            this.review = review;
        }
     
        [AllowAnonymous]//It allows any user, including unauthenticated ones, to access the endpoint.
        [HttpPost]
        [Route("Register Customer")]
        public IActionResult CreateUser([FromQuery] UserDto userDto)
        {

            var createdUser = userService.CreateUser(userDto);
            if (createdUser == null)
            {
                return NotFound("You dont have access to create admin");
            }
            return Ok(createdUser);
        }
        [Authorize(Roles = "admin,customer,restaurant,deliveryagent")]
        [HttpPut("update/userProfile")]
        public IActionResult UpdateUser([FromQuery] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("User Updation Cannot be done");
            }
            var updatedUser = userServices.UpdateUser(userDto);
            return Ok(updatedUser);
        }
        [Authorize(Roles = "admin,customer,restaurant,deliveryagent")]
        [HttpGet("Saved Addresses")]
        public IActionResult GetAllAddresses([FromQuery] string phno)
        {
           List<AddressDto> address = userServices.GetAddressesByUserId(phno);
            return Ok(address);
        }
        [Authorize(Roles = "admin,customer")]
        [HttpGet("Orders")]
        public IActionResult GetOrdersByUser([FromQuery] string phno)
        {
            List<GetOrderDto> orders = userServices.GetOrdersByUserId(phno);
            return Ok(orders);
        }
        [Authorize(Roles = "customer")]
        [HttpPost("OrderNow")]
        public IActionResult AddingOrder([FromBody] AddOrderDto dto)
        {
            if (dto.UserId <= 0 || dto.RestaurantId <= 0)
            {
                return BadRequest("Invalid order data.");
            }

            order.AddOrder(dto);
            return Ok("Order added successfully.");
        }

        [HttpGet]
        public IActionResult GetRestaurants()
        {
            var restaurants = restaurant.GetRestaurants();
            if (restaurants == null)
            {
                return NotFound();
            }
            return Ok(restaurants);
        }
        [Authorize(Roles = "Admin,customer")]
        [HttpGet("SearchFoodItems")]
        public IActionResult GetFoodItemsByKeywords(string keyword)
        {
            var foodItems = foodItemsService.GetFoodItemsByKeywords(keyword);
            if (foodItems == null)
            {
                return NotFound("No food items by the keyword");
            }
            return Ok(foodItems);
        }
        [Authorize(Roles = "customer,admin")]
        [HttpGet("get/foodItemsByRestaurant/{name}")]
        public IActionResult GetFoodItemsByRestaurant([FromRoute] string name)
        {
            var foodItemsByRestaurant = foodItemsService.GetFoodItemsByRestaurant(name);
            return Ok(foodItemsByRestaurant);
        }
        [Authorize(Roles = "admin,customer,restaurant,deliveryagent")]
        [HttpGet("GetReviewsByRestaurant/{restaurantName}")]
        public IActionResult GetReviewsByRestaurant(string restaurantName)
        {
            var result = review.GetAverageRatingByRestaurantName(restaurantName);
            return Ok(result);
        }

        [Authorize(Roles = "admin,customer,restaurant,deliveryagent")]
        [HttpGet("Getrestaurantbyrating/{rating}")]
        public IActionResult GetrestaurantbyRating(decimal rating)
        {
            IEnumerable<ReviewGetRestaurantDto> restaurants = review.GetRestaurantNameByRating(rating);
            return Ok(restaurants);
        }

    }
}
