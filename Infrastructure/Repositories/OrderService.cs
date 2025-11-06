using Domain.Data;
using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using FoodItemDto = Domain.DTO.FoodItemDto;

namespace Infrastructure.Repositories
{
    public class OrderService : IOrder
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }



        //public void AddOrder(AddOrderDto dto)
        //{
        //    var newOrder = new Order
        //    {
        //        UserId = dto.UserId,
        //        RestaurantId = dto.RestaurantId
        //    };

        //    _context.Orders.Add(newOrder);
        //    _context.SaveChanges();
        //}




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

        public void AddOrder(AddOrderDto dto)
        {
            var newOrder = new Order
            {
                UserId = dto.UserId,
                RestaurantId = dto.RestaurantId,
                CustAddressId = dto.CustAddressId,
                Status = dto.Status,
                CreatedAt = dto.CreatedAt
            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges(); // OrderId is now available

            foreach (var item in dto.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = newOrder.OrderId,       // ✅ FK to Orders
                    ItemId = item.ItemId,         // ✅ FK to FoodItems
                    Quantity = item.Quantity
                };

                _context.OrderItems.Add(orderItem);
            }

            try
            {
                _context.SaveChanges(); // Save all order items
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("EF Core Save Error:");
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }




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
            .Select(oi => new FoodDeliveryProject.DTOs.FoodItemDtoWithPrice
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
            id= orderId,
            RestaurantName = order.Restaurant?.User?.Name ?? "Unknown",
            CustomerName = order.User?.Name ?? "Unknown",
            TotalPrice = totalPrice,
            FoodItems = foodItems
        };
    }
        
       
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
                    .Select(oi => new FoodItemDtoWithPrice
                    {
                        ItemName=oi.Item.ItemName,
                        Price = oi.Item.Price,
                        Quantity=oi.Quantity,
                        TotalItemPrice = oi.Item.Price * oi.Quantity
                    })
                    .ToList();

                var totalPrice = foodItems.Sum(fi => fi.TotalItemPrice);

                result.Add(new GetOrderDto
                {
                    id=order.OrderId,
                    RestaurantName = order.Restaurant?.User?.Name ?? "Unknown",
                    CustomerName = order.User?.Name ?? "Unknown",
                    TotalPrice = totalPrice,
                    FoodItems = foodItems
                });
            }

            return result;
        }


        public List<GetOrderDto> GetIncomingOrdersForRestaurant(int restaurantId)
        {
            var orders = _context.Orders
                .Where(o => o.RestaurantId == restaurantId &&
                            (o.Status == "Pending" || o.Status == "Placed"))
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
                    .Select(oi => new FoodItemDtoWithPrice
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
                    id = order.OrderId,
                    RestaurantName = order.Restaurant?.User?.Name ?? "Unknown",
                    CustomerName = order.User?.Name ?? "Unknown",
                    TotalPrice = totalPrice,
                    FoodItems = foodItems,
                    Status = order.Status
                });
            }

            return result;
        }
        public bool UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null) return false;

            order.Status = status;
            _context.SaveChanges();
            return true;
        }



        public List<ActiveOrderDto> GetActiveOrdersByAgentId(int agentId)
        {
            var deliveries = _context.Deliveries
                .Include(d => d.Order)
                    .ThenInclude(o => o.User)
                        .ThenInclude(u => u.Addresses)
                .Include(d => d.Order)
                    .ThenInclude(o => o.Restaurant)
                        .ThenInclude(r => r.User)
                        .ThenInclude(u => u.Addresses)

                  .Include(d => d.Order)
                    // <-- Add this line

                .Where(d => d.AgentId == agentId && d.Status == true && (d.Order.Status == "Pending"||d.Order.Status=="Accepted"))
                .ToList();

            var result = new List<ActiveOrderDto>();

            foreach (var delivery in deliveries)
            {
                var order = delivery.Order;
                if (order == null) continue;

                // Get order items manually
                var orderItems = _context.OrderItems
                    .Where(oi => oi.OrderId == order.OrderId)
                    .Join(_context.FoodItems,
                          oi => oi.ItemId,
                          fi => fi.ItemId,
                          (oi, fi) => new { fi.Price, oi.Quantity })
                    .ToList();

                var totalPrice = order.OrderItems;
                var customerAddress = order.User?.Addresses?.FirstOrDefault()?.Address1 ?? "N/A";
                var restaurantAddress = order.Restaurant?.User?.Addresses?.FirstOrDefault()?.Address1 ?? "N/A";


                result.Add(new ActiveOrderDto
                {
                    OrderId = order.OrderId,
                    Status = order.Status,
                    RestaurantName = order.Restaurant?.User?.Name ?? "Unknown",
                    RestaurantAddress = restaurantAddress,
                    CustomerName = order.User?.Name ?? "Unknown",
                    CustomerAddress = customerAddress,
                    
                });
            }

            return result;
        }
        public bool UpdateOrderStatusToDelivered(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return false;

            if (order.Status == "Pending"||order.Status=="Accepted")
            {
                order.Status = "Delivered";
                _context.SaveChanges();
                return true;
            }

            return false; // Optional: only allow update if status is currently "Pending"
        }

    }

}
