//using Domain.Models;
//using Infrastructure.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace FoodDeliveryProject.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class DeliveryyController : ControllerBase
//    {
//        private readonly OpenRouteServiceClient _orsClient;

//        public DeliveryyController(OpenRouteServiceClient orsClient)
//        {
//            _orsClient = orsClient;
//        }

//        [HttpPost("eta-customer-agent")]
//        public async Task<IActionResult> GetETAForDelivery([FromBody] DeliveryETARequest request)
//        {
//            if (string.IsNullOrWhiteSpace(request.CustomerAddress) || string.IsNullOrWhiteSpace(request.AgentAddress))
//                return BadRequest("Both customer and agent addresses are required.");

//            var customerCoords = await _orsClient.GeocodeAddressAsync(request.CustomerAddress);
//            var agentCoords = await _orsClient.GeocodeAddressAsync(request.AgentAddress);

//            var (distance, duration) = await _orsClient.GetRouteAsync(
//                agentCoords.Lat, agentCoords.Lng,
//                customerCoords.Lat, customerCoords.Lng
//            );

//            return Ok(new
//            {
//                CustomerAddress = request.CustomerAddress,
//                AgentAddress = request.AgentAddress,
//                DistanceInKm = distance / 1000,
//                DurationInMinutes = duration / 60
//            });
//        }
//    }
//}
