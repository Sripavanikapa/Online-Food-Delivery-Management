using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class CategoryDto
    {
      
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CategoryFormDto
    {
        [FromForm(Name = "Name")]
        public string Name { get; set; } = string.Empty;
        [FromForm(Name = "Image")]
        public IFormFile Image { get; set; }
      

    }
}