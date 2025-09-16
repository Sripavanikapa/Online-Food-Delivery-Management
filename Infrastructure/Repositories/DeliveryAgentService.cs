using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.ADO;
using Infrastructure.Interfaces;
using Domain.DTO;

namespace Infrastructure.Repositories
{
    public class DeliveryAgentService: IDeliveryAgent
    {
        //for delivery agent
        public List<AllDeliveryDto> GetDeliveriesByAgentPhone(string phoneno)
        {
            var deliveries = new List<AllDeliveryDto>();

            using (SqlConnection conn = SqlConn.GetConnection())
            {
                string query = "SELECT d.delivery_id, d.order_id, d.status " +
                    "FROM Delivery d " +
                    "join users u on u.id = d.agent_id " +
                    "WHERE u.phoneno = @phoneno";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@phoneno", phoneno);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    deliveries.Add(new AllDeliveryDto
                    {
                        DeliveryId = Convert.ToInt32(reader["delivery_id"]),
                        OrderId = Convert.ToInt32(reader["order_id"]),
                        Status = (bool)reader["status"]
                    });
                }
                return deliveries;

            }
        }

        //for delivery agent
        public bool UpdateDeliveryStatus(int DeliveryId)
        {
            using (SqlConnection conn = SqlConn.GetConnection())
            {
                string query = "Update delivery set status =1 where delivery_id=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", DeliveryId);
                cmd.ExecuteNonQuery();
            }
            return true;
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
