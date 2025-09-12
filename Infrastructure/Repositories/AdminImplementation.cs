using Domain.Data;
using Domain.DTO;
using Domain.Models;

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

        public AdminImplementation(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public List<RestaurantDto> getAllRestaurants()
        {
          
            return appDbContext.Restaurants.Select(u=>new RestaurantDto
            {
                RestaurantId=u.RestaurantId,
                OwnerName=u.User.Name,
                Status = u.Status
            }).ToList();
        
        }

        public int getAllUsersCount()
        {
            return appDbContext.Users.Count();
        }
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
        public bool UpdateUserStatus(int id, bool isValid)
        {
            var user = appDbContext.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return false;
            }
            user.IsValid = isValid;
            appDbContext.SaveChanges();
            return true;
        }
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
        public List<DeliveryAgentDto> getDeliveryAgents()
        {
            return appDbContext.DeliveryAgents.Select(u=>new DeliveryAgentDto
            {
                AgentId=u.AgentId,
               
                Status=u.Status
            }).ToList();
        }

        public List<UserDto> getCustomers()
        {
            var Customers = appDbContext.Users.Where(u => u.Role == "Customer")
                .Select(x => new UserDto
                {
                    
                    Name = x.Name,
                    Phoneno = x.Phoneno,
                    IsValid = x.IsValid,
                    Role=x.Role
                }).ToList();
            return Customers;
        }
    }
}
