using Domain.DTO;
using Domain.Models;
using Infrastructure.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TokenGeneration _tokenGeneration;

        public AuthController(IConfiguration configuration, TokenGeneration tokenGeneration)
        {
            _configuration = configuration;
            _tokenGeneration = tokenGeneration;

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] LoginDto loginRequest)
        {

            var response = await _tokenGeneration.Authenticate(loginRequest);
            if (response == null)
                return Unauthorized(new { Message = "Invalid Phone Number or password" });

            return Ok(response);
        }

    }
}
