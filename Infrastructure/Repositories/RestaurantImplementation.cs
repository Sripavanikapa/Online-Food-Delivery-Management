using Domain.Data;
using Domain.DTO;
using Domain.Models;


using Microsoft.EntityFrameworkCore;


namespace FoodDeliveryProject.Repositories
{
    public class RestaurantImplementation : IRestaurant
    {
        
        private readonly AppDbContext appDbContext;

        public RestaurantImplementation(AppDbContext appDbContext)
        {
           
            this.appDbContext = appDbContext;
        }
        public List<Restaurant> GetRestaurants()
        {
            return appDbContext.Restaurants.ToList();
        }
        public RestaurantDto AddRestaurant(RestaurantCreateDto restaurantCreateDto)
        {
            var user = appDbContext.Users.Find(restaurantCreateDto.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var restaurant = new Restaurant
            {
                Status=restaurantCreateDto.Status,
                User=user,
            };
            appDbContext.Restaurants.Add(restaurant);
            appDbContext.SaveChanges();

            return new RestaurantDto
            {
                RestaurantId = restaurant.RestaurantId,
                Status = restaurant.Status,
                OwnerName = user.Name
            };
        }
        public RestaurantDto GetRestaurantById(int id)
        {
            return appDbContext.Restaurants
                 .Where(r => r.RestaurantId == id)
                 .Select(r => new RestaurantDto
                 {
                     RestaurantId = r.RestaurantId,
                     Status = r.Status,
                     OwnerName = r.User.Name
                 })
                 .FirstOrDefault();

        }
        public bool UpdateRestaurantStatus(int id,bool status)
        {
            var restaurant = appDbContext.Restaurants.FirstOrDefault(x=>x.RestaurantId==id);
            if(restaurant == null)
            {
                return false;
            }
            restaurant.Status=status;
            appDbContext.SaveChanges();
            return true;
        }

        public bool DeleteRestaurant(int id)
        {
            var restaurantToBeDeleted= appDbContext.Restaurants.FirstOrDefault(x=>x.RestaurantId== id);
            if(restaurantToBeDeleted == null)
            {
                return false;
            }
            appDbContext.Remove(restaurantToBeDeleted);
            appDbContext.SaveChanges();
            return true;
        }
        //public List<RestaurantDto> GetRestaurantByRole(string role)
        //{
        //    return appDbContext.Restaurants
        //        .Where(r => r.User.Role == role)
        //        .Select(r => new RestaurantDto
        //        {
        //            RestaurantId = r.User.Id,
        //            Status = r.Status,
        //            OwnerName = r.User.Name
        //        }).ToList();

        //}

        public List<RestaurantWithFoodDto> RestaurantWithThereFoodItems()
        {
            var restaurants = appDbContext.Restaurants
                .Include(r => r.FoodItems)
                .Include(r => r.User)
                .ToList();

            return restaurants.Select(r => new RestaurantWithFoodDto
            {
                RestaurantId = r.RestaurantId,
                OwnerName = r.User?.Name ?? "Unknown",
                Status = r.Status,
                FoodItems = r.FoodItems != null && r.FoodItems.Any()
                    ? r.FoodItems.Select(f => new FoodItemDtoForRestaurant
                    {
                        ItemId = f.ItemId,
                        ItemName = f.ItemName ?? string.Empty,
                        Price = f.Price,
                        Rating = f.Rating,
                        Description = f.Description ?? string.Empty,
                        Keywords = f.Keywords ?? string.Empty,
                        Status = f.Status
                    }).ToList()
                    : "No food item"
            }).ToList();
        }


    }
}
