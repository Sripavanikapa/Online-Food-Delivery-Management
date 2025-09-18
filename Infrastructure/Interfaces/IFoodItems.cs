using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Models;

namespace Infrastructure.Interfaces
{
    public interface IFoodItems
    {
        bool DeleteFoodItemByFoodId(int item_id);
        FoodItemDto CreateFoodItem(FoodItemDto fooditem);
        public IEnumerable<FoodByKeywords> GetFoodItemsByKeywords(string keywords);

        FoodItemDto UpdateFoodItem(int foodid,FoodItemDto fooditem);

        List<FoodItemDto> GetFoodItemsByRestaurant(string name);
    }
}
