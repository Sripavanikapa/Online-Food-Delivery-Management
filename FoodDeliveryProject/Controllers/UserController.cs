using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.DTO;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;

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
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser([FromQuery] UserDto userDto)
        {
           
            var createdUser = userServices.CreateUser(userDto);
            if (createdUser==null)
            {
                return NotFound("You dont have access to create admin");
            }
            return Ok(createdUser);
        }

        [HttpPut("update/user")]
        public IActionResult UpdateUser([FromQuery] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("User Updation Cannot be done");
            }
            var updatedUser = userServices.UpdateUser(userDto);
            return Ok(updatedUser);
        }

        [HttpGet("get/alladdress/users")]
        public IActionResult GetAllUsers([FromQuery]string phno)
        {
            IEnumerable<Address> address = userServices.GetAddressesByUserId(phno);
            return Ok(address);
        }


        [HttpGet("getorders")]
        public IActionResult GetOrdersByUser([FromQuery]string phno)
        {
            IEnumerable<OrderedItemsByUserDto> orders = userServices.GetOrdersByUserId(phno);
            return Ok(orders);
        }

        [HttpDelete("delete/user")]
        public IActionResult DeleteUser([FromQuery] string Phno)
        {
            bool isDeleted = userServices.DeleteUser(Phno);
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
