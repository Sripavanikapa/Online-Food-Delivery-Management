using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Domain.ADO
{
    public static class SqlConn
    {
        private static readonly string _connectionString = "Data Source = LTIN523324\\SQLEXPRESS;Initial Catalog = FoodDelivery; Integrated Security = True; TrustServerCertificate=true";

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
