using Domain.Data;
using Domain.DTO;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FoodItemServices : IFoodItems
    {
        private readonly AppDbContext _context;
        public FoodItemServices(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<FoodByKeywords> GetFoodItemsByKeywords(string keyword)
        {
            return _context.FoodByKeywords
                .FromSqlRaw("EXEC proc_fooditems_by_keywords @keyword = {0}", keyword)
                .AsEnumerable()
                .ToList();
        }

        public FoodItemDto CreateFoodItem(FoodItemDto fooditem)
        {


            var food = new FoodItem
            {

                RestaurantId = fooditem.restaurant_id,
                ItemName = fooditem.item_name,
                Price = fooditem.price,
                Rating = 0,
                CategoryId = fooditem.category_id,
                Status = fooditem.status,
                Description = fooditem.Description,
                Keywords = fooditem.keywords,



            };

            _context.FoodItems.Add(food);
            _context.SaveChanges();

            return new FoodItemDto
            {
                restaurant_id = food.RestaurantId,
                item_name = food.ItemName,
                price = food.Price,
               
                category_id = food.CategoryId,
                status = food.Status,
                Description = food.Description,
                keywords = food.Keywords
            };
        }

        public bool DeleteFoodItemByFoodId(int item_id)
        {
            var item = _context.FoodItems.FirstOrDefault(f => f.ItemId == item_id);
            if (item != null)
            {
                _context.FoodItems.Remove(item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public FoodItemDto UpdateFoodItem(int itemid, FoodItemDto fooditem)
        {
            var food = _context.FoodItems.Find(itemid);
            if (food != null)
            {

                food.RestaurantId = fooditem.restaurant_id;
                food.ItemName = fooditem.item_name;
                food.Price = fooditem.price;
              
                food.CategoryId = fooditem.category_id;
                food.Status = fooditem.status;
                food.Description = fooditem.Description;
                food.Keywords = fooditem.keywords;


                return new FoodItemDto
                {
                    restaurant_id = food.RestaurantId,
                    item_name = food.ItemName,
                    price = food.Price,
                  
                    category_id = food. CategoryId,
                    status = food.Status,
                    Description = food.Description,
                    keywords = food.Keywords,

                };
            }
            return null;
        }

    }
}
