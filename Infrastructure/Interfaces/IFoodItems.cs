using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IFoodItems
    {
        bool DeleteFoodItemByFoodId(int item_id);
        FoodItemDto CreateFoodItem(FoodItemDto fooditem);
        public IEnumerable<FoodByKeywords> GetFoodItemsByKeywords(string keywords);

        FoodItemDto UpdateFoodItem(int foodid, FoodItemDto fooditem);

        List<FoodItemDto> GetFoodItemsByRestaurant(string name);
        Task<FoodItemDto> UploadFoodItemAsync(FoodItemDto dto, IFormFile image);
        Task<FoodItemDto> UpdateFoodItemWithImageAsync(int itemid, FoodItemDto dto, IFormFile image);
        FoodItemDto GetFoodItemById(int foodid);
        Task<IEnumerable<FoodItemDto>> SearchFoodItemsAsync(string keyword);
        List<FoodItemDto> GetFoodItemsByCategory(string categoryName);
    }
}
