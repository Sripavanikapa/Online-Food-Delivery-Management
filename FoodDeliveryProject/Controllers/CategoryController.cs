using Domain.DTO;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController()
        {
            _categoryService= new CategoryService();
        }


        // get all categories


        [Authorize(Roles = "admin,customer,restaurant")]
        [HttpGet("AllCategories")]
        public ActionResult<List<string>> GetAllCategories()
        {
            List<string> categories = _categoryService.GetAllCategories();
            if (categories == null)
            {
                return NotFound($"No category found");
            }
            return Ok(categories);
        }


        // add category

        [Authorize(Roles ="restaurant")]
        [HttpPost("AddCategory")]
        public ActionResult AddCategory([FromBody] CategoryDto catagoryDto) { 
            var category=_categoryService.AddCategory(catagoryDto);
            if (category == null)
            {
                return BadRequest("");
            }
            return Ok(category);
        }

       

    }
}
