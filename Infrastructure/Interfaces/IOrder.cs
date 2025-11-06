using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IOrder
    {
        public void AddOrder(AddOrderDto dto);
        //public void AddOrder(int userId, int restaurantId);

        //public IEnumerable<Order> GetOrderByUserId(int userId);

        //public IEnumerable<Order> GetOrderByRestaurantId(int restaurantId);

        //public object GetOrderByOrderId(int orderId);

        public GetOrderDto GetOrderByOrderId(int orderId);
        public List<GetOrderDto> GetIncomingOrdersForRestaurant(int restaurantId);
        bool UpdateOrderStatus(int orderId, string status);
        public List<ActiveOrderDto> GetActiveOrdersByAgentId(int agentId);
        public bool UpdateOrderStatusToDelivered(int orderId);
    }
}
