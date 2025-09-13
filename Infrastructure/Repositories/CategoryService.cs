using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.ADO;
using Domain.DTO;
using Domain.Data;

namespace Infrastructure.Repositories
{
    public class CategoryService
    {
        //private readonly AppDbContext _appDbContext;
        public List<string> GetAllCategories()
        {
            List<string> categories = new List<string>();
            using (SqlConnection conn = SqlConn.GetConnection())
            {
                string query = "Select name from category";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    categories.Add(reader["name"].ToString());              
            }
                reader.Close();
            }
            return categories;
        }
       
        public CategoryDto AddCategory(CategoryDto categoryDto)
        {
            using (SqlConnection conn = SqlConn.GetConnection()) 
            {
                string query = "Insert into category(name) values(@Name)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", categoryDto.Name);
                cmd.ExecuteNonQuery();
            }
           return new CategoryDto { Name = categoryDto.Name };
        }
    }
}
