using Domain.ADO;
using Domain.Data;
using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CategoryService
    {
        private readonly AppDbContext _appDbContext;
        public CategoryService(AppDbContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        public List<CategoryDto> GetAllCategories()
        {
            return _appDbContext.Categories
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl
                })
                .ToList();
        }


        public CategoryDto AddCategory(CategoryFormDto categoryFormDto)
        {
            var image = categoryFormDto.Image;
            if (image == null || image.Length == 0)
                throw new Exception("Image is required");

            var fileName = $"{Guid.NewGuid()}_{image.FileName}";

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "category");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            // Save relative path to DB
            var relativePath = Path.Combine("images/category", fileName).Replace("\\", "/");

            var category = new Category
            {
                Name = categoryFormDto.Name,
                ImageUrl = relativePath
            };

            _appDbContext.Categories.Add(category);
            _appDbContext.SaveChanges();

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = categoryFormDto.Name,
                ImageUrl = relativePath
            };
        }
       
        public CategoryDto UpdateCategory(int categoryId, CategoryFormDto categoryFormDto)
        {
            var image = categoryFormDto.Image;
            if (image == null || image.Length == 0)
                throw new Exception("Image is required");
            // Fetch category by ID
            var category = _appDbContext.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (category == null)
                throw new Exception("Category not found");

            // Update name
            if (!string.IsNullOrWhiteSpace(categoryFormDto.Name))
            {
                category.Name = categoryFormDto.Name;
            }

            // Update image if provided
            if (image != null && image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "category");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream); // Async for better performance
                }

                // Save relative path for web access
                var relativePath = Path.Combine("images/category", fileName).Replace("\\", "/");
                category.ImageUrl = relativePath;
            }

            _appDbContext.SaveChanges();

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ImageUrl = category.ImageUrl
            };

        }

    }
}
