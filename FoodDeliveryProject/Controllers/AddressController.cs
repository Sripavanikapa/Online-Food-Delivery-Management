using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.DTO;
using Infrastructure.Interfaces;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly  IAddress _address;
        public AddressController(IAddress address)
        {
            _address = address;
        }


        //create a address
        [HttpPost("create/address")]
        public IActionResult CreateAddress([FromQuery] AddressDto address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAddress = _address.CreateAddress(address);
            return Ok(createdAddress);
        }


        //update a address
        [HttpPut("update/address")] 
        public IActionResult UpdateAddress([FromQuery] int addressid,[FromQuery] AddressDto address)
        {
            if (!ModelState.IsValid)
            {
                return NotFound(ModelState);
            }
            var updatedAddress = _address.UpdateAddress(addressid,address);
            return Ok(updatedAddress);
        }


        //get addresses by customer id
        [HttpGet("get/addresses")]

        public ActionResult GetAddressesByCustomerId([FromQuery]string phno)
        {
            var addresses = _address.GetAddressesByPhno(phno);
            if (addresses == null || addresses.Count == 0)
            {
                return NotFound("No addresses found for the provided customer ID");
            }
            return Ok(addresses);
        }

        //delete address by id

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
