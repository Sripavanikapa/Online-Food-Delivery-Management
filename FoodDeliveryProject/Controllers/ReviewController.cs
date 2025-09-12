using Domain.DTO;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReview _rating;

        public ReviewController(IReview rating)
        {
            _rating = rating;
        }

        
        [HttpGet("GetReviewsByRestaurant/{restaurantName}")]
        public IActionResult GetReviewsByRestaurant(string restaurantName)
        {
            var result = _rating.GetAverageRatingByRestaurantName(restaurantName);
            return Ok(result);
        }



        [HttpGet("Getrestaurantbyrating/{id}")]
        public IActionResult GetrestaurantbyRating(decimal id)
        {
            IEnumerable<ReviewGetRestaurantDto> restaurants = _rating.GetRestaurantNameByRating(id);
            return Ok(restaurants);
        }

    }
}
