using Domain.Data;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;


namespace Infrastructure.Repositories
{
    public class UserServices : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserServices(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public UserDto CreateUser(UserDto userdto)
        {
            var user = new User
            {
                Name = userdto.Name,
                Phoneno = userdto.Phoneno,
                Password = userdto.Password,
                IsValid = false,
                Role = userdto.Role
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return new UserDto
            {
                Name = user.Name,
                Phoneno = user.Phoneno,
                Password = user.Password,
                IsValid = user.IsValid,
                Role = user.Role
            };
        }

        public IEnumerable<Address> GetAddressesByUserId(int userId)
        {
            var user = _context.Addresses.Find(userId);
            if (user != null)
            {
                return _context.Addresses.Where(a => a.CustId == userId).ToList();
            }
            return Enumerable.Empty<Address>();
        }


        //get orders by user id
        public IEnumerable<OrderedItemsByUserDto> GetOrdersByUserId(int userid)
        {
            return _context.OrderDetailsByUserId.FromSqlRaw("exec proc_get_ordersby_userid @userid={0}", userid)
                .ToList();

        }

        public UserDto UpdateUser(int id, UserDto userdto)
        {
            var existingAddress = _context.Users.Find(id);
            if (existingAddress != null)
            {
                existingAddress.Name = userdto.Name;
                existingAddress.Phoneno = userdto.Phoneno;
                existingAddress.Password = userdto.Password;
                existingAddress.IsValid = false;
                existingAddress.Role = userdto.Role;

                _context.SaveChanges();

                return new UserDto
                {
                    Name = existingAddress.Name,
                    Phoneno = existingAddress.Phoneno,
                    Password = existingAddress.Password,
                    IsValid = existingAddress.IsValid,
                    Role = existingAddress.Role
                };
            }

            return null;


        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<RestaurantWithFoodDto> RestaurantWithThereFoodItems()
        {
            var restaurants = _context.Restaurants
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
