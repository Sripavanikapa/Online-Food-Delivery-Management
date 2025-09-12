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

        //for allocating
        public List<DeliveryAgentDto> GetAvailableAgents()
        {
            List<DeliveryAgentDto> agents = new List<DeliveryAgentDto>();
            using (SqlConnection conn = SqlConn.GetConnection())
            {
                string query = "Select agent_id, status from delivery_agent where status=1";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    agents.Add(new DeliveryAgentDto
                    {
                        AgentId = Convert.ToInt32(reader["agent_id"]),
                        //Status =(bool) reader["status"]
                    });
                }
            }
            return agents;
        }

    }
}

