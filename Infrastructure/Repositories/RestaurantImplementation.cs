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
        //public List<RestaurantDto> GetRestaurants()
        //{
        //    return appDbContext.Restaurants.Select(r => new RestaurantDto
        //    {
        //        OwnerName = r.User.Name
        //    }
        //    ).ToList();
        //}
        public List<string> GetRestaurants()
        {
            return appDbContext.Restaurants
                .Select(r => r.User.Name)
                .Distinct()
                .ToList();
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
                     OwnerName = r.User.Name,
                     IsValid=r.User.IsValid,

                     Phoneno = r.User.Phoneno,
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
                        Status = f.Status,
                        ImageUrl = f.ImageUrl

                    }).ToList()
                    : "No food item"
            }).ToList();
        }
        //used sql in linq
        public List<RestaurantDto> GetOpenRestaurants()
        {
            var restaurants = appDbContext.Restaurants
                .FromSqlRaw("SELECT restaurant_id, status FROM Restaurant WHERE status = 'true'")
                .Select(r => new RestaurantDto
                {
                    RestaurantId = r.RestaurantId,
                    Status = r.Status,
                    OwnerName=r.User.Name
                })
                .ToList();

            return restaurants;
        }

        public RestaurantDto GetRestaurantByUserId(int userId)
        {
            var restaurant = appDbContext.Restaurants
                .Include(r => r.User) 
                .Include(r=>r.User.Addresses)
                .FirstOrDefault(r => r.User.Id == userId); 

            if (restaurant == null)
            {
                Console.WriteLine($"No restaurant found for userId: {userId}");
                return null;
            }

            return new RestaurantDto
            {
                RestaurantId = restaurant.RestaurantId,
                OwnerName = restaurant.User?.Name ?? "Unknown", 
                Status = restaurant.Status,
                Phoneno=restaurant.User.Phoneno,
                IsValid=restaurant.User.IsValid,
                Address=restaurant.User.Addresses.FirstOrDefault()?.Address1,
            };
        }
        public List<OrderHistoryDto> GetOrderHistoryForRestaurant(int restaurantId)
        {
            var orders = appDbContext.Orders
                .Where(o => o.RestaurantId == restaurantId)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            var result = new List<OrderHistoryDto>();

            foreach (var order in orders)
            {
                var customer = appDbContext.Users.FirstOrDefault(u => u.Id == order.UserId);
                var items = appDbContext.OrderItems
                    .Where(oi => oi.OrderId == order.OrderId)
                    .Join(appDbContext.FoodItems,
                          oi => oi.ItemId,
                          fi => fi.ItemId,
                          (oi, fi) => new OrderedItemDto
                          {
                              ItemName = fi.ItemName,
                              Quantity = oi.Quantity,
                              Price = fi.Price
                          }).ToList();

                result.Add(new OrderHistoryDto
                {
                    OrderId = order.OrderId,
                    CustomerName = customer?.Name ?? "Unknown",
                    Status = order.Status,
                    OrderDate=order.CreatedAt,
                    Items = items
                });
            }

            return result;
        }



    }
}
