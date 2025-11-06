using Domain.Data;
using Domain.DTO;

using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryProject.Repositories
{
    public class OrderItemService : IOrderItem
    {
        private readonly AppDbContext _context;

        public OrderItemService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderItemDto> GetFoodByOrderId(int id)
        {
            var foodItems = _context.OrderItems
                .Include(oi => oi.Item)
                .Where(oi => oi.OrderId == id && oi.Item != null)
                .Select(oi => new OrderItemDto
                {
                  
                    Quantity = oi.Quantity,
                   
                })
                .ToList();

            return foodItems;
        }
    }
}
