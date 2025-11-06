using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;
using FoodDeliveryProject.Repositories;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

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
        private readonly IAddress addressService;

        public CustomerController(IFoodItems foodItemsService,IUserRepository userService,IRestaurant restaurant,IUserRepository userServices,IOrder order,IReview review,IAddress addressService)
        {
            this.foodItemsService = foodItemsService;
            this.userService = userService;
            this.restaurant = restaurant;
            this.userServices = userServices;
            this.order = order;
            this.review = review;
            this.addressService = addressService;
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
        [Authorize(Roles = "customer,restaurant,deliveryagent")]
        [HttpPost("Add/address")]
        public IActionResult CreateAddress([FromQuery] AddressDto address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAddress = addressService.CreateAddress(address);
            return Ok(createdAddress);
        }


        //update a address
        [Authorize(Roles = "customer,restaurant,deliveryagent")]
        [HttpPut("update/address")]
        public IActionResult UpdateAddress([FromQuery] int addressid, [FromQuery] AddressDto address)
        {
            if (!ModelState.IsValid)
            {
                return NotFound(ModelState);
            }
            var updatedAddress = addressService.UpdateAddress(addressid, address);
            return Ok(updatedAddress);
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
        [Authorize(Roles = "customer,restaurant,deliveryagent")]
        [HttpDelete("delete/address")]
        public ActionResult DeleteAddressById([FromQuery] int id)
        {
            var result = addressService.DeleteAddressById(id);
            if (result == false)
            {
                return BadRequest("Provided id is not found");
            }
            return Ok(result);
        }
        //[Authorize(Roles = "admin,customer")]
        //[HttpGet("Orders/{phno}")]
        //public IActionResult GetOrdersByUserPhno([FromRoute] string phno)
        //{
        //    List<GetOrderDto> orders = userServices.GetOrdersByUserId(phno);
        //    return Ok(orders);
        //}
        [HttpPost("OrderNow")]
        public IActionResult AddingOrder([FromBody] AddOrderDto dto)
        {






            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            order.AddOrder(dto);
            return Ok("Order placed successfully");
        }


        [Authorize(Roles = "customer,admin,restaurant")]
        [HttpGet("YourOrderInfo/{id}")]
        public IActionResult GetOrderByOrderId(int id)
        {
            var orders =order.GetOrderByOrderId(id);

            if (orders == null)
                return NotFound("Order not found.");

            return Ok(orders);
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
      
        [HttpGet("Orders")]
        public IActionResult GetOrdersByUserId(int userId)
        {
            List<GetOrderDto> orders = userServices.GetOrdersByUserId(userId);
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }
            return Ok(orders);
        }



    }
}
