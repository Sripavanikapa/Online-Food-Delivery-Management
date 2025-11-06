using Domain.DTO;
using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly  IAddress _address;
        private readonly IUserRepository userService;

        public AddressController(IAddress address, IUserRepository userService)
        {
            _address = address;
            this.userService = userService;
        }





        ////create a address
        //[Authorize(Roles = "customer,restaurant,deliveryagent")]
        //[HttpPost("create/address")]
        //public IActionResult CreateAddress([FromBody] AddressDto address)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var createdAddress = _address.CreateAddress(address);
        //    return Ok(createdAddress);
        //}

        [HttpPost("create/address")]
        public IActionResult CreateAddress(AddressDto address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAddress = _address.CreateAddress(address);
            return Ok(createdAddress);
        }

        [Authorize(Roles = "customer,restaurant,deliveryagent")]

        //update a address
        [HttpPut("update/address")]
        public IActionResult UpdateAddress([FromQuery] int addressid, [FromQuery] string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return BadRequest("Address cannot be empty.");
            }

            try
            {
                var updatedAddress = _address.UpdateAddress(addressid, address);
                return Ok(updatedAddress);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //get addresses by customer id
        [Authorize(Roles = "customer,restaurant,deliveryagent")]
        [HttpGet("get/addresses")]

        public ActionResult<AddressDto> GetAddressesByCustomerId([FromQuery]int userid)
        {
            var addresses = _address.GetAddressesByPhno(userid);
            if (addresses == null || addresses.Count == 0)
            {
                return NotFound("No addresses found for the provided customer ID");
            }
            return Ok(addresses);
        }




        //delete address by id
        [HttpGet("get/alladdress/users")]
        public IActionResult GetAllUsers([FromQuery] int userid)
        {
            List<AddressShowing> address = userService.GetAddressesByUserPhno(userid);
            return Ok(address);
        }


        [Authorize(Roles = "customer,restaurant,deliveryagent")]
        [HttpDelete("delete/address")]
        public ActionResult DeleteAddressById([FromQuery]int id)
        {
            var result = _address.DeleteAddressById(id);
            if (result == false)
            {
                return BadRequest("Provided id is not found");
            }
            return Ok(result);
        }
    }
}
