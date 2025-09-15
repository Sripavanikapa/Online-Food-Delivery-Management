using Domain.Data;
using Domain.DTO;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ReviewService : IReview
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        //public decimal GetAverageRatingByRestaurantName(string restaurantName)
        //{
        //    var reviews = _context.Reviews
        //        .Include(r => r.Restaurant)
        //        .ThenInclude(res => res.User)
        //        .ToList(); // Fetch data into memory

        //    var averageRating = reviews
        //        .Where(r => r.Restaurant != null &&
        //                    r.Restaurant.User.Name == restaurantName) // Case-sensitive match
        //        .Select(r => r.Rating)
        //        .DefaultIfEmpty(0)
        //        .Average();

        //    return (decimal)averageRating;
        //}


        
        public ReviewGetRatingdto GetAverageRatingByRestaurantName(string restaurantName)
        {
            var reviews = _context.Reviews
                .Include(r => r.Restaurant)
                .ThenInclude(res => res.User)
                .ToList();

            var averageRating = reviews
                .Where(r => r.Restaurant != null && r.Restaurant.User.Name == restaurantName)
                .Select(r => r.Rating)
                .DefaultIfEmpty(0)
                .Average();

            return new ReviewGetRatingdto
            {
                RestaurantName = restaurantName,
                AverageRating = (decimal)averageRating
            };
        }


        public IEnumerable<ReviewGetRestaurantDto> GetRestaurantNameByRating(decimal rating)
        {
            var restaurantNames = _context.Reviews
                .Include(r => r.Restaurant)
                .ThenInclude(res => res.User)
                .Where(r => r.Rating == rating && r.Restaurant != null)
                .Select(r => new ReviewGetRestaurantDto
                {
                    RestaurantName = r.Restaurant.User.Name
                })
                .Distinct()
                .ToList();

            return restaurantNames;
        }

    }
}
