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
            var existingUser = _context.Users.FirstOrDefault(u => u.Phoneno == userdto.Phoneno);
            if (existingUser != null)
            {
                
                return new UserDto { Role = "duplicate" };
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
                    
                    Address1 = s.Address1
                }).ToList();
        }
        public List<GetOrderDto> GetOrdersByUserId(int userId)
        {
            if (userId == 0)
                return new List<GetOrderDto>();

            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
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
                        TotalItemPrice = oi.Item.Price * oi.Quantity,
                        ImageUrl=oi.Item.ImageUrl

                    })
                    .ToList();

                var totalPrice = foodItems.Sum(fi => fi.TotalItemPrice);

                result.Add(new GetOrderDto
                {
                    RestaurantName = order.Restaurant.User.Name,
                    CustomerName = order.User.Name,
                    TotalPrice = totalPrice,
                    FoodItems = foodItems,
                    Status=order.Status,
                    CreatedAt=order.CreatedAt
                });
            }

            return result;
        }


        //get orders by user id
        //public List<GetOrderDto> GetOrdersByUserPhno(string phno)
        //{
        //    var userId = _context.Users
        //        .Where(u => u.Phoneno == phno)
        //        .Select(u => u.Id)
        //        .FirstOrDefault();

        //    if (userId == 0)
        //        return new List<GetOrderDto>();

        //    var orders = _context.Orders
        //        .Where(o => o.UserId == userId)
        //        .Include(o => o.User)
        //        .Include(o => o.Restaurant)
        //            .ThenInclude(r => r.User)
        //        .ToList();

        //    var result = new List<GetOrderDto>();

        //    foreach (var order in orders)
        //    {
        //        var foodItems = _context.OrderItems
        //            .Where(oi => oi.OrderId == order.OrderId)
        //            .Include(oi => oi.Item)
        //            .Select(oi => new FoodDeliveryProject.DTOs.FoodItemDtoWithPrice
        //            {
        //                ItemName = oi.Item.ItemName,
        //                Price = oi.Item.Price,
        //                Quantity = oi.Quantity,
        //                TotalItemPrice = oi.Item.Price * oi.Quantity
        //            })
        //            .ToList();

        //        var totalPrice = foodItems.Sum(fi => fi.TotalItemPrice);

        //        result.Add(new GetOrderDto
        //        {
        //            RestaurantName = order.Restaurant.User.Name,
        //            CustomerName = order.User.Name,
        //            TotalPrice = totalPrice,
        //            FoodItems = foodItems
        //        });
        //    }

        //    return result;
        //}


        public UpdateUserDto UpdateUser(UpdateUserDto user)
        {
            var existingAddress = _context.Users.FirstOrDefault(u=>u.Phoneno == user.Phoneno);
            if (existingAddress != null)
            {
                existingAddress.Name = user.Name;
                existingAddress.Phoneno = user.Phoneno;
                

                _context.SaveChanges();

                return new UpdateUserDto
                {
                    Name = existingAddress.Name,
                    Phoneno = existingAddress.Phoneno,

               
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



        public UserInfoDto GetUserInfoByUserid(int userid)
        {
            var user = _context.Users.Find(userid);
            return new UserInfoDto
            {
                Phno = user.Phoneno,
                Name = user.Name
            };
        }
        public List<AddressShowing> GetAddressesByUserPhno(int userid)
        {


            var addresses = _context.Addresses
                            .Where(a => a.CustId == userid)
                            .Select(a => new AddressShowing
                            {
                                AddressId = a.AddressId,
                                Address = a.Address1
                            })
                            .ToList();

            if (addresses != null) return addresses;

            return null;
        }

        public List<OrderedItemsByUserDto> GetOrderedItems(int userid)
        {
            throw new NotImplementedException();
        }
        public bool ForgotPassword(ForgotPasswordDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Phoneno == dto.Phoneno);
            if (user == null)
                return false;

            user.Password = dto.NewPassword;
            _context.SaveChanges();
            return true;
        }
    }
}
