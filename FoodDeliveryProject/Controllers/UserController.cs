using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.DTO;
using Infrastructure.Interfaces;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userServices;


        public UserController(IUserRepository userServices)
        {
            this.userServices = userServices;
        }

        [HttpPost]
        public IActionResult CreateUser([FromQuery] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdUser = userServices.CreateUser(userDto);
            return Ok(createdUser);
        }

        [HttpPut("update/user")]
        public IActionResult UpdateUser([FromQuery] int id, [FromQuery] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("User Updation Cannot be done");
            }
            var updatedUser = userServices.UpdateUser(id, userDto);
            return Ok(updatedUser);
        }

        [HttpGet("get/alladdress/users/{id}")]
        public IActionResult GetAllUsers(int id)
        {
            IEnumerable<Address> address = userServices.GetAddressesByUserId(id);
            return Ok(address);
        }


        [HttpGet("getorders/{userid}")]
        public IActionResult GetOrdersByUser(int userid)
        {
            IEnumerable<OrderedItemsByUserDto> orders = userServices.GetOrdersByUserId(userid);
            return Ok(orders);
        }

        [HttpDelete("delete/user/{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool isDeleted = userServices.DeleteUser(id);
            if (isDeleted)
            {
                return Ok("User deleted successfully.");
            }
            else
            {
                return NotFound("User not found.");
            }
        }



    }
}
