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
        private readonly CategoryService categoryService;

        public AdminController(IAdmin admin,CategoryService categoryService)
        {
            this.admin = admin;
            this.categoryService = categoryService;
        }

        // get total users count


        [Authorize(Roles = "admin")]
        [HttpGet("/TotalUsers")]
        public ActionResult<int> getAllUsersCount()
        {
            return admin.getAllUsersCount();
        }

        // get total restaurants count

        [Authorize(Roles = "admin")]
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



        //[HttpPut]
        //[Authorize(Roles = "admin")]
        //[Route("ApproverOrBlockUser/{id}/{isValid}")]
        //public IActionResult ApproverOrBlockRestaurant([FromRoute] int id, [FromRoute] bool isValid)
        //{
        //    var updated=admin.UpdateUserStatus(id, isValid);
        //    if (!updated)
        //    {
        //        return NotFound(new { message = "not updated user details" });
        //    }
        //    return Ok(new { message = "user details updated successfully" });
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

        [Authorize(Roles = "admin")]
        [HttpPost("AddCategory")]
        public ActionResult AddCategory([FromBody] CategoryDto catagoryDto)
        {
            var category = categoryService.AddCategory(catagoryDto);
            if (category == null)
            {
                return BadRequest("");
            }
            return Ok(category);
        }

        [Authorize(Roles = "admin,customer,restaurant")]
        [HttpGet("AllCategories")]
        public ActionResult<List<string>> GetAllCategories()
        {
            List<string> categories = categoryService.GetAllCategories();
            if (categories == null)
            {
                return NotFound($"No category found");
            }
            return Ok(categories);
        }

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
        [Authorize(Roles = "admin")]
        [HttpPut("{id}/{isValid}/approve")]
     
        public async Task<IActionResult> ApproveUser(int id, bool isValid)
        {
            try
            {
                var result = await admin.ApproveUserAsync(id, isValid);

                if (result.Contains("customer", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { message = result });
                }

                return Ok(new { message = "sms sent to user"});
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
