using Domain.Data;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace FoodDeliveryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService _categoryService)
        {
            this._categoryService = _categoryService;
        }


        // get all categories





        // add category
        [Authorize(Roles = "admin")]

        [HttpPost("AddCategory")]
        public ActionResult AddCategory([FromForm] CategoryFormDto categoryFormDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = _categoryService.AddCategory(categoryFormDto);
            return Ok(category);
        }
        [HttpGet("AllCategories")]
        public ActionResult<List<CategoryDto>> GetAllCategories()
        {
            List<CategoryDto> categories = _categoryService.GetAllCategories();
            if (categories == null || categories.Count == 0)
            {
                return NotFound("No categories found");
            }
            return Ok(categories);
        }


        //[HttpPost("UpdateCategory/{id}")]
        //[Authorize(Roles = "admin")]
        //public ActionResult<CategoryDto> UpdateCategory(int id, [FromForm] string name, [FromForm] IFormFile? image)
        //{
        //    try
        //    {
        //        var form = new CategoryFormDto { Name = name };
        //        var updated = _categoryService.UpdateCategory(id, form, image);
        //        return Ok(updated);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}
        [HttpPut("UpdateCategory/{id}")]

        public ActionResult UpdateCategory([FromRoute] int id, [FromForm] CategoryFormDto categoryFormDto)

        {

            try

            {

                var updatedCategory = _categoryService.UpdateCategory(id, categoryFormDto);

                return Ok(updatedCategory);

            }

            catch (Exception ex)

            {

                return BadRequest(new { message = ex.Message });

            }

        }



    }
}
