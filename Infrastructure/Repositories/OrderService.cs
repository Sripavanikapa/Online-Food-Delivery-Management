using Domain.Data;
using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class OrderService : IOrder
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }


        
        public void AddOrder(AddOrderDto dto)
        {
            var newOrder = new Order
            {
                UserId = dto.UserId,
                RestaurantId = dto.RestaurantId
            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();
        }


        //public object GetOrderByOrderId(int orderId)
        //{
        //    // Load the order with related User and Restaurant (and Restaurant's User)
        //    var order = _context.Orders
        //        .Include(o => o.User)
        //        .Include(o => o.Restaurant)
        //            .ThenInclude(r => r.User)
        //        .FirstOrDefault(o => o.OrderId == orderId);

        //    if (order == null)
        //        return null;

        //    // Load the order items separately
        //    var foodItems = _context.OrderItems
        //        .Where(oi => oi.OrderId == orderId)
        //        .Include(oi => oi.Item)
        //        .Select(oi => new
        //        {
        //            oi.Item.ItemName,
        //            oi.Item.Price,
        //            oi.Quantity,
        //            TotalItemPrice = oi.Item.Price * oi.Quantity
        //        })
        //        .ToList();

        //    var totalPrice = foodItems.Sum(fi => fi.TotalItemPrice);

        //    return new
        //    {
        //        RestaurantName = order.Restaurant?.User?.Name ?? "Unknown",
        //        CustomerName = order.User?.Name ?? "Unknown",
        //        TotalPrice = totalPrice,
        //        FoodItems = foodItems
        //    };
        //}

        

    public GetOrderDto GetOrderByOrderId(int orderId)
    {
        var order = _context.Orders
            .Include(o => o.User)
            .Include(o => o.Restaurant)
                .ThenInclude(r => r.User)
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
            return null;

        var foodItems = _context.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .Include(oi => oi.Item)
            .Select(oi => new FoodDeliveryProject.DTOs.FoodItemDto
            {
                ItemName = oi.Item!.ItemName,
                Price = oi.Item.Price,
                Quantity = oi.Quantity,
                TotalItemPrice = oi.Item.Price * oi.Quantity
            })
            .ToList();

        var totalPrice = foodItems.Sum(fi => fi.TotalItemPrice);

        return new GetOrderDto
        {
            RestaurantName = order.Restaurant?.User?.Name ?? "Unknown",
            CustomerName = order.User?.Name ?? "Unknown",
            TotalPrice = totalPrice,
            FoodItems = foodItems
        };
    }


}
}
