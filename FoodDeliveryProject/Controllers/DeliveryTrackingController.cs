using Domain.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryTrackingController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ITrackingApiClient trackingApiClient;
        private readonly OpenRouteServiceClient routeService;

        public DeliveryTrackingController(AppDbContext context, ITrackingApiClient trackingApiClient, OpenRouteServiceClient routeService)
        {
            this.context = context;
            this.trackingApiClient = trackingApiClient;
            this.routeService = routeService;
        }
       

            [HttpGet("track/{orderId}")]
            public async Task<IActionResult> TrackDelivery(int orderId)
            {
                try
                {
                    var trackingInfo = await trackingApiClient.TrackDeliveryByOrderIdAsync(orderId);
                    return Ok(trackingInfo);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }
        }
    }



