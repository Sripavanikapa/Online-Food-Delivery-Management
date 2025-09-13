using Domain.ADO;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class DeliveryServices: IDelivery
    {
        public DeliveryDto GetDeliveryDetailsByOrderId(int id, string CustAddress)
        {
            DeliveryDto delivery = null;

            using (SqlConnection conn = SqlConn.GetConnection())
            {
                string query = "getDeliveryDetailsByOrderId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", id);
                cmd.Parameters.AddWithValue("@CustAddress", CustAddress);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    delivery = new DeliveryDto
                    {

                        DeliveryId = Convert.ToInt32(reader["delivery_id"]),
                        OrderId = Convert.ToInt32(reader["order_id"]),
                        restaurantName = reader["RestaurantName"].ToString(),
                        restaurantAddress = reader["RestaurantAddress"].ToString(),
                        customerName = reader["CustName"].ToString(),
                        customerAddress = reader["CustAddress"].ToString(),
                        Status = Convert.ToBoolean(reader["status"])

                    };
                }
                reader.Close();
            }
            return delivery;
        }

        

    }
}

