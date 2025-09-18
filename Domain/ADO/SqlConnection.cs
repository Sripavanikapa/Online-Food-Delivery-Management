using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Domain.ADO
{
    public static class SqlConn
    {
        private static readonly string _connectionString = "Data Source=localhost;User=SA;Password=password-1;Initial Catalog=FoodDeliveryManagement;TrustServerCertificate=true";

        //private static string _connectionString;

        //public static void Initialize(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("DefaultConnection");
        //}

        public static SqlConnection GetConnection(){

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public static void CloseConnection(SqlConnection connection){

            connection.Close();
        }
    }
}
