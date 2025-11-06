using Domain.Data;
using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.Repositories;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
//        USE[Food]

//GO

///****** Object:  StoredProcedure [dbo].[proc_get_ordersby_userid]    Script Date: 23-10-2025 10:21:56 ******/

//SET ANSI_NULLS ON

//GO

//SET QUOTED_IDENTIFIER ON

//GO

//ALTER procedure[dbo].[proc_get_ordersby_userid]

//        @userid int

//as

//begin

//SELECT

//    f.item_name,

//    f.imageurl,

//    u.name,

//    f.price,

//    f.rating,

//    o.status,

//    o.Date

//FROM  orders o

//JOIN food_items f ON o.item_id = f.item_id

//JOIN restaurant r ON f.restaurant_id = r.restaurant_id

//JOIN users u ON r.restaurant_id = u.id

//WHERE o.user_id = @userid;

//        end

//        fooditemsbykeywords


//USE[Food]

//GO

///****** Object:  StoredProcedure [dbo].[proc_fooditems_by_keywords]    Script Date: 23-10-2025 10:22:25 ******/

//SET ANSI_NULLS ON

//GO

//SET QUOTED_IDENTIFIER ON

//GO

//ALTER procedure[dbo].[proc_fooditems_by_keywords]

//        @keyword varchar(255)

//as

//begin

//    select i.item_name,u.name,i.price,i.Description,i.imageurl,i.item_id,i.restaurant_id

//    from food_items i

//    join

//    restaurant r

//    on

//    i.restaurant_id = r.restaurant_id

//    join

//    users u

//    on

//    r.restaurant_id=u.id

//    WHERE i.keywords LIKE '%'+@keyword+'%'

//end

//exec proc_fooditems_by_keywords chicken



        public FoodItemDto CreateFoodItem(FoodItemDto fooditem)
        {


            var food = new FoodItem
            {

                RestaurantId = fooditem.restaurant_id,
                ItemName = fooditem.itemName,
                Price = fooditem.price,
                Rating = 0,
                CategoryId = fooditem.category_id,
                Status = fooditem.status,
                Description = fooditem.Description,
                Keywords = fooditem.keywords,
                ImageUrl=fooditem.imageurl,


            };

            _context.FoodItems.Add(food);
            _context.SaveChanges();

            return new FoodItemDto
            {
                restaurant_id = food.RestaurantId,
                itemName = food.ItemName,
                price = food.Price,
               
                category_id = food.CategoryId,
                status = food.Status,
                Description = food.Description,
                keywords = food.Keywords,

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
        public FoodItemDto UpdateFoodItem(int foodid, FoodItemDto fooditem)
        {
            var food = _context.FoodItems.Find(foodid);
            if (food == null) return null;

            food.ItemName = fooditem.itemName;
            food.Price = fooditem.price;
            food.Description = fooditem.Description;
            food.RestaurantId = fooditem.restaurant_id;
            food.CategoryId = fooditem.category_id;
            food.Status = fooditem.status;
            food.Keywords = fooditem.keywords;
            food.ImageUrl = fooditem.imageurl;

            _context.SaveChanges();

            return new FoodItemDto
            {
                restaurant_id = food.RestaurantId,
                itemName = food.ItemName,
                price = food.Price,
                category_id = food.CategoryId,
                status = food.Status,
                Description = food.Description,
                keywords = food.Keywords,
                imageurl = food.ImageUrl
            };
        }


        public async Task<FoodItemDto> UpdateFoodItemWithImageAsync(int itemid, FoodItemDto dto, IFormFile image)
        {
            var food = await _context.FoodItems.FindAsync(itemid);
            if (food == null)
                return null;

            // Save new image if provided
            if (image != null && image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                var filePath = Path.Combine("wwwroot/images/fooditems", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                food.ImageUrl = $"/images/fooditems/{fileName}";
            }

            // Update other fields
            food.ItemName = dto.itemName;
            food.Price = dto.price;
            food.Description = dto.Description;
            food.RestaurantId = dto.restaurant_id;
            food.CategoryId = dto.category_id;
            food.Status = dto.status;
            food.Keywords = dto.keywords;

            await _context.SaveChangesAsync();

            return new FoodItemDto
            {
                restaurant_id = food.RestaurantId,
                itemName = food.ItemName,
                price = food.Price,
                category_id = food.CategoryId,
                status = food.Status,
                Description = food.Description,
                keywords = food.Keywords,
                imageurl = food.ImageUrl
            };
        }

        public async Task<FoodItemDto> UploadFoodItemAsync(FoodItemDto dto, IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new Exception("Image is required");

            var fileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine("wwwroot/images/fooditems", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var food = new FoodItem
            {
                ItemName = dto.itemName,
                Price = dto.price,
                Description = dto.Description,
                RestaurantId = dto.restaurant_id,
                CategoryId = dto.category_id,
                Status = dto.status,
                Keywords = dto.keywords,
                ImageUrl = $"/images/fooditems/{fileName}",
                Rating=0
               
            };

            _context.FoodItems.Add(food);
            await _context.SaveChangesAsync();

            return new FoodItemDto
            {
                restaurant_id = food.RestaurantId,
                itemName = food.ItemName,
                price = food.Price,
                category_id = food.CategoryId,
                status = food.Status,
                Description = food.Description,
                keywords = food.Keywords,
                imageurl = food.ImageUrl
            };
        }

        public List<FoodItemDto> GetFoodItemsByRestaurant(string name)
        {
            var foodItems = _context.FoodItems
                .Where(f => f.Restaurant.User.Name == name)
                .Select(f => new FoodItemDto
                {
                    restaurant_id = f.RestaurantId,
                    itemName = f.ItemName,
                    price = f.Price, 
                    category_id = f.CategoryId,
                    status = f.Status,
                    Description = f.Description,
                    keywords = f.Keywords,
                    imageurl = f.ImageUrl
                })
                .ToList();

            return foodItems;
        }

        public FoodItemDto GetFoodItemById(int foodid)
        {

            var food = _context.FoodItems.FirstOrDefault(f => f.ItemId == foodid);

            return new FoodItemDto
            {
                ItemId=food.ItemId,
                itemName = food.ItemName,
                restaurant_id = food.RestaurantId,
                price = food.Price,
                category_id = food.CategoryId,
                Description = food.Description,
                imageurl = food.ImageUrl,
                keywords = food.Keywords,
                status = food.Status
            };
        }
        public async Task<IEnumerable<FoodItemDto>> SearchFoodItemsAsync(string keyword)
        {
            return await _context.FoodItems
                .Where(f => f.ItemName.Contains(keyword) || f.Description.Contains(keyword) || f.Keywords.Contains(keyword))
                .Select(f => new FoodItemDto
                {
                    ItemId = f.ItemId,
                    restaurant_id = f.RestaurantId,
                    itemName = f.ItemName,
                    price = f.Price,
                    Description = f.Description,
                    imageurl = f.ImageUrl
                })
                .ToListAsync();
        }
        public List<FoodItemDto> GetFoodItemsByCategory(string categoryName)
        {
            var items = _context.FoodItems
                .Where(fi => fi.Category.Name == categoryName)
                .Select(fi => new FoodItemDto
                {
                    ItemId = fi.ItemId,
                    itemName = fi.ItemName,
                    price = fi.Price,
                    Description = fi.Description,
                    imageurl = fi.ImageUrl,
                    CategoryName = fi.Category.Name
                })
                .ToList();

            return items;
        }
    }
}
