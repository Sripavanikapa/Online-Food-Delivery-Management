using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IAdmin
    {
        int getAllUsersCount();
       // List<UserDto> getUsersByRole(string role);

        List<RestaurantDto> getAllRestaurants();

        bool UpdateUserStatus(int id, bool isValid);

      //  bool DeleteRestauarnt(int id);

        bool DeleteUser(int id);

        List<RestaurantOrderSummaryDto> restaurantOrderSummaryDto();
       // bool UpdateDeliveryAgentStatus(int id , bool status);

        List<DeliveryAgentDto> getDeliveryAgents();

        List<UserDto> getCustomers();

    }
}
