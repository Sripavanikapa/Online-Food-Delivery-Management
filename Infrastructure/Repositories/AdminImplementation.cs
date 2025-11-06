using Domain.Data;
using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AdminImplementation : IAdmin
    {
        private readonly AppDbContext appDbContext;
        private readonly ISmsService smsService;
        public AdminImplementation(AppDbContext appDbContext, ISmsService smsService)
        {
            this.appDbContext = appDbContext;
            this.smsService = smsService;
            
        }

        public List<RestaurantDto> getAllRestaurants()
        {
            return appDbContext.Restaurants
                .Select(r => new RestaurantDto
                {
                    RestaurantId = r.RestaurantId,
                    OwnerName = r.User.Name,
                    Phoneno = r.User.Phoneno,
                    Address = r.User.Addresses
                        .Select(a => a.Address1)
                        .FirstOrDefault() ?? "N/A",
                    Status = r.Status,
                    IsValid = r.User.IsValid
                }).ToList();
        }


        public int getAllUsersCount()
        {
            return appDbContext.Users.Count();
        }
        #region UnUsedCode
        //List<UserDto> IAdmin.getUsersByRole(string role)
        //{
        //    var users = appDbContext.Users.Where(u => u.Role == role)
        //        .Select(u => new UserDto
        //        {
        //            Id = u.Id,
        //            Name = u.Name,
        //            Phoneno = u.Phoneno,
        //            Role = u.Role,
        //            IsValid = u.IsValid
        //        }).ToList();
        //    return users;
        //}
        //public bool UpdateUserStatus(int id, bool isValid)
        //{
        //    var user = appDbContext.Users.FirstOrDefault(x => x.Id == id);
        //    if (user == null)
        //    {
        //        return false;
        //    }
        //    user.IsValid = isValid;
        //    appDbContext.SaveChanges();
        //    return true;
        //} 
        #endregion
        public async Task<string> ApproveUserAsync(int id,bool isValid)
        {
            var user = await appDbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            if (user.Role == "customer")
            {
                return "Customers need not be approved";
            }
            user.IsValid = isValid;
            appDbContext.Users.Update(user);
            await appDbContext.SaveChangesAsync();


            string msg = isValid ? "Your Account has been approved by Admin." :
                "Your Account has been blocked by Admin.";
            if (!string.IsNullOrWhiteSpace(user.Phoneno))
            {
                await smsService.SendSmsAsync(
                    user.Phoneno,
                    msg
                    );
            }
            return msg;
        }
        #region UnUsedCode
        //public bool DeleteRestauarnt(int id)
        //{
        //    var restaurantToBeDelete=appDbContext.Restaurants.FirstOrDefault(x=>x.RestaurantId == id);
        //    if(restaurantToBeDelete == null)
        //    {
        //        return false;
        //    }
        //    appDbContext.Remove(restaurantToBeDelete);
        //    appDbContext.SaveChanges();
        //    return true;
        //} 
        #endregion
        public bool DeleteUser(int id)
        {
            var userToBeDeleted=appDbContext.Users.FirstOrDefault(x=>x.Id == id);
            if(userToBeDeleted == null)
            {
                return false;
            }
            appDbContext.Remove(userToBeDeleted);
            appDbContext.SaveChanges();
            return true;
        }
        List<RestaurantOrderSummaryDto> IAdmin.restaurantOrderSummaryDto()
        {
            var result = appDbContext.Orders.GroupBy(o => o.RestaurantId)
                .Select(g => new RestaurantOrderSummaryDto
                {
                    RestaurantId = g.Key,
                    RestaurantName = g.Select(o => o.Restaurant != null ? o.Restaurant.User.Name : "UnKnown")
                    .FirstOrDefault() ?? "Unknown",
                    NoOfOrders = g.Count()
                }).ToList();
            return result;
        }
        #region UnUsed
        //public bool UpdateDeliveryAgentStatus(int id,bool status)
        //{
        //    var agent = appDbContext.DeliveryAgents.Find(id);
        //    if (agent == null)
        //    {
        //        return false;
        //    }
        //    agent.Status = status;
        //    appDbContext.SaveChanges();
        //    return true;
        //} 
        #endregion
        public List<DeliveryAgentDto> getDeliveryAgents()
        {
            return appDbContext.DeliveryAgents
                .Select(u => new DeliveryAgentDto
                {
                    AgentId = u.AgentId,
                    AgentName = u.Agent.Name,
                    Phoneno = u.Agent.Phoneno,
                    IsValid = u.Agent.IsValid,
                    IsActive = u.Agent.IsActive // or derive from status if needed
                }).ToList();
        }

        public List<AdminUser> getCustomers()
        {
            var Customers = appDbContext.Users.Where(u => u.Role == "Customer")
                .Select(x => new AdminUser
                {
                    
                    Id=x.Id,
                    Name = x.Name,
                    Phoneno = x.Phoneno,
                    IsValid = x.IsValid,
                    Role=x.Role
                }).ToList();
            return Customers;
        }
        public int getAllRestaurantsCount()

        {

            return appDbContext.Users

           .Where(u => u.Role == "restaurant")

           .Count();

        }
        public int getAllOrdersCount()

        {

            return appDbContext.Orders.Count();

        }
        public List<RestaurantOrderCountDto> Top5Restaurant()

        {

            var joinedData = appDbContext.Users.Join(appDbContext.Orders,

                user => user.Id,

                order => order.RestaurantId,

                (user, order) => new

                {

                    user.Id,

                    user.Name,

                    order

                }

            );

            var groupedCounts = joinedData.GroupBy(

                x => new { x.Id, x.Name },

                (key, g) => new RestaurantOrderCountDto

                {

                    Name = key.Name,

                    Orders = g.Count()

                }

            );

            var orderedResults = groupedCounts.OrderByDescending(dto => dto.Orders);

            var top5Results = orderedResults.Take(5);

            return top5Results.ToList();
        }
        public List<GetOrderDto> GetAllOrders()
        {
            // Preload deliveries with full navigation to DeliveryAgent and User
            var deliveries = appDbContext.Deliveries
                .Include(d => d.Agent)
                    .ThenInclude(da => da.Agent) // da.Agent is the User
                .ToList();

            // Preload orders with related data
            var orders = appDbContext.Orders
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                    .ThenInclude(r => r.User)
                .Include(o => o.OrderItems)
                .ToList();

            // Project into DTO
            var result = orders.Select(order =>
            {
                var delivery = deliveries.FirstOrDefault(d => d.OrderId == order.OrderId);

                var totalPrice = order.OrderItems
                    .Join(appDbContext.FoodItems,
                          oi => oi.ItemId,
                          fi => fi.ItemId,
                          (oi, fi) => fi.Price * oi.Quantity)
                    .Sum();

                return new GetOrderDto
                {
                    id = order.OrderId,
                    CustomerName = order.User.Name,
                    RestaurantName = order.Restaurant.User.Name,
                    AgentId = delivery?.AgentId ?? 0,
                    AgentName = delivery?.Agent?.Agent?.Name ?? "Unassigned",
                    TotalPrice = totalPrice
                };
            }).ToList();

            return result;
        }





    }
}
