using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        //create user


        [AllowAnonymous]
        [HttpPost("register")]

        public IActionResult CreateUser([FromBody] UserDto userDto)
        {

            var createdUser = userServices.CreateUser(userDto);

            if (createdUser == null)
            {
                return BadRequest(new { message = "You don't have access to create admin." });
            }

            if (createdUser.Role == "duplicate")
            {
                return Conflict(new { message = "A user with this phone number is already registered." });
            }

            return Ok(createdUser);
        }
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var result = userServices.ForgotPassword(dto);

            if (!result)
                return NotFound(new { message = "No user found with this phone number." });

            return Ok(new { message = "Password updated successfully." });
        }



        //update user


        [Authorize(Roles = "admin,customer,restaurant,deliveryagent")]
        [HttpPut("update/user")]
        public IActionResult UpdateUser([FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("User Updation Cannot be done");
            }
            var updatedUser = userServices.UpdateUser(userDto);
            return Ok(updatedUser);
        }
       


        //get all addresses by user phone number


        [Authorize(Roles = "admin,customer,restaurant,deliveryagent")] 
        [HttpGet("get/alladdress/users")]
        public IActionResult GetAllUsers([FromQuery]string phno)
        {
            List<AddressDto> address = userServices.GetAddressesByUserId(phno);
            return Ok(address);
        }


        //get all orders by user phone number

        //[Authorize(Roles = "admin,customer")]
        //[HttpGet("getorders")]
        //public IActionResult GetOrdersByUserPhno([FromQuery]string phno)
        //{
        //    List<GetOrderDto> orders = userServices.GetOrdersByUserId(phno);
        //    return Ok(orders);
        //}
        [Authorize(Roles = "admin,customer")]
        [HttpGet("getorders")]
        public IActionResult GetOrdersByUserId([FromQuery] int userId)
        {
            List<GetOrderDto> orders = userServices.GetOrdersByUserId(userId);
            return Ok(orders);
        }

        //delete user by phone number

        [Authorize(Roles = "admin,customer,restaurant,deliveryagent")]
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

        [HttpGet("get/userInfoById")]
        public IActionResult GetUser(int userid)
        {
            UserInfoDto user = userServices.GetUserInfoByUserid(userid);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(user);
        }



    }
}
