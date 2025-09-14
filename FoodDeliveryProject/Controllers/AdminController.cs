using Domain.Data;
using Domain.DTO;
using Domain.Models;

using FoodDeliveryProject.Repositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly IAdmin admin;
        public AdminController(IAdmin admin)
        {
            this.admin = admin;
            
        }
        [Authorize]
        [HttpGet("/TotalUsers")]
        public ActionResult<int> getAllUsersCount()
        {
            return admin.getAllUsersCount();
        }

        //[HttpGet]
        //[Route("byrole{role}")]
        //public ActionResult<List<UserDto>> getUsersByRole(string role)
        //{
        //    return admin.getUsersByRole(role);

        //}
        [Authorize]
        [HttpGet("/getCustomers")]
        public ActionResult<List<AdminUser>> GetCustomers()
        {
            var Customers = admin.getCustomers();
            if (Customers == null)
            {
                return NotFound();
            }
            return Ok(Customers);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("/getRestaurants")]
        public  ActionResult<List<RestaurantDto>> GetRestaurants()
        {
            var restaurants = admin.getAllRestaurants();
            if (restaurants == null)
            {
                return NotFound();
            }
            return Ok(restaurants);
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("ApproverOrBlockUser/{id}/{isValid}")]
        public IActionResult ApproverOrBlockRestaurant([FromRoute] int id, [FromRoute] bool isValid)
        {
            var updated=admin.UpdateUserStatus(id, isValid);
            if (!updated)
            {
                return NotFound(new { message = "not updated user details" });
            }
            return Ok(new { message = "user details updated successfully" });
        }
        //[HttpDelete]
        //[Route("/restaurant/{id}")]
        //public IActionResult DeleteRestauarnt([FromRoute] int id)
        //{
        //    var deleteRestaurant=admin.DeleteRestauarnt(id);
        //    if (!deleteRestaurant)
        //    {
        //        return NotFound(new { message = "No User found with that ID" });
        //    }
        //    return Ok(new { message = "Successfully Deleted that restaurant" });
        //}
        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("user/{id}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            var userToBeDeleted=admin.DeleteUser(id);
            if (!userToBeDeleted)
            {
                return NotFound(new { message = "No User found with that id" });
            }
            return Ok(new { message = "User successfully Deleted" });
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("ordersSummary")]
        public ActionResult<List<RestaurantOrderSummaryDto>> GetOrdersCountByRestaurant()
        {
            var data = admin.restaurantOrderSummaryDto();
            if (!data.Any())
            {
                return NotFound(new { message = "no orders found" });
            }
            return Ok(data);
        }
        //[HttpPut("UpdateOrBlockDeliveryAgent")]
        //public IActionResult UpdateDeliveryAgentStatus(int id,bool status)
        //{
        //    var agent=admin.UpdateDeliveryAgentStatus(id,status);
        //    if (!agent)
        //    {
        //        return NotFound("No agent with that id");
        //    }
        //    return Ok("status updated successfully");
        //}
        [Authorize(Roles = "admin")]
        [HttpGet("getAllDeliveryAgents")]
        public IActionResult getDeliveryAgents()
        {
            var deliveryAgents=admin.getDeliveryAgents();
            if (deliveryAgents == null)
            {
                return NotFound();
            }
            return Ok(deliveryAgents);
        }
    }
}
