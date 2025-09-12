using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IReview
    {
        //public decimal GetAverageRatingByRestaurantName(string restaurantName);

        public ReviewGetRatingdto GetAverageRatingByRestaurantName(string restaurantName);

        //public IEnumerable<string> GetRestaurantNameByRating(decimal rating);

        public IEnumerable<ReviewGetRestaurantDto> GetRestaurantNameByRating(decimal rating);
    }
}
