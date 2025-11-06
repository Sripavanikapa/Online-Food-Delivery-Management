using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.Interfaces
{
    public interface IOrderItem
    {
        
        public IEnumerable<OrderItemDto> GetFoodByOrderId(int id);
    }
}
