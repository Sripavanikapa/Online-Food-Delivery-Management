using Domain.Data;
using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


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
            if(userdto.Role == "admin")
            {
                return null;
            }
            var user = new User
            {
              
                Name = userdto.Name,
                Phoneno = userdto.Phoneno,
                Password = userdto.Password,
                IsValid = false,
                Role = userdto.Role,
                IsActive=false
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return new UserDto
            {
                Name = user.Name,
                Phoneno = user.Phoneno,
                Password = user.Password,
                Role = user.Role
            };
        }

        public List<AddressDto> GetAddressesByUserId(string phno)
        {
            var userId = _context.Users
                .Where(u => u.Phoneno == phno)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (userId == 0)
                return new List<AddressDto>();

            return _context.Addresses
                .Where(a => a.CustId == userId)
                .Select(s=>new AddressDto
                {
                    Phno=s.Cust.Phoneno,
                    Address1 = s.Address1
                }).ToList();
        }


        //get orders by user id
        public List<GetOrderDto> GetOrdersByUserId(string phno)
        {
            var userId = _context.Users
                .Where(u => u.Phoneno == phno)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (userId == 0)
                return new List<GetOrderDto>();

            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Restaurant)
                    .ThenInclude(r => r.User)
                .ToList();

            var result = new List<GetOrderDto>();

            foreach (var order in orders)
            {
                var foodItems = _context.OrderItems
                    .Where(oi => oi.OrderId == order.OrderId)
                    .Include(oi => oi.Item)
                    .Select(oi => new FoodDeliveryProject.DTOs.FoodItemDtoWithPrice
                    {
                        ItemName = oi.Item.ItemName,
                        Price = oi.Item.Price,
                        Quantity = oi.Quantity,
                        TotalItemPrice = oi.Item.Price * oi.Quantity
                    })
                    .ToList();

                var totalPrice = foodItems.Sum(fi => fi.TotalItemPrice);

                result.Add(new GetOrderDto
                {
                    RestaurantName = order.Restaurant.User.Name,
                    CustomerName = order.User.Name,
                    TotalPrice = totalPrice,
                    FoodItems = foodItems
                });
            }

            return result;
        }


        public UpdateUserDto UpdateUser(UpdateUserDto user)
        {
            var existingAddress = _context.Users.FirstOrDefault(u=>u.Phoneno == user.Phoneno);
            if (existingAddress != null)
            {
                existingAddress.Name = user.Name;
                existingAddress.Phoneno = user.Phoneno;
                existingAddress.Password = user.Password;

                _context.SaveChanges();

                return new UpdateUserDto
                {
                    Name = existingAddress.Name,
                    Phoneno = existingAddress.Phoneno,
                    Password = existingAddress.Password,
               
                };
            }

            return null;


        }

        public bool DeleteUser(string phno)
        {
            var user = _context.Users.FirstOrDefault(u=> u.Phoneno == phno);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }



    }
}
