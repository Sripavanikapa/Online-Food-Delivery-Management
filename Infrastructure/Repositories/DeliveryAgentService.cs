using Domain.ADO;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DeliveryAgentService : IDeliveryAgent
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

        public string GetAgentName(int AgentId)
        {
            using (SqlConnection conn = SqlConn.GetConnection())
            {
                string query = "SELECT u.name FROM users u JOIN delivery_agent da ON u.id = da.agent_id WHERE da.agent_id = @AgentId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AgentId", AgentId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return reader["name"].ToString();
                }
                else
                {
                    return null!;
                }
            }
        }

        public string GetAgentPhone(int AgentId)
        {
            using (SqlConnection conn = SqlConn.GetConnection())
            {
                string query = "SELECT u.phoneno FROM users u JOIN delivery_agent da ON u.id = da.agent_id WHERE da.agent_id = @AgentId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AgentId", AgentId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return reader["phoneno"].ToString();
                }
                else
                {
                    return null!;
                }
            }
        }

        public bool UpdateAgentStatus(int AgentId, bool status)
        {
            using (SqlConnection conn = SqlConn.GetConnection())
            {
                string query = "Update delivery_agent set status =@status where agent_id=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", AgentId);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();
            }
            return true;
        }
        public List<OrderHistoryDto> GetOrderHistoryForAgent(int agentId)
        {
            var orders = new List<OrderHistoryDto>();

            using (SqlConnection conn = SqlConn.GetConnection())
            {
                string query = @"
            SELECT o.order_id, o.CreatedAt, o.status, u.name AS customer_name
            FROM delivery d
            JOIN orders o ON o.order_id = d.order_id
            JOIN users u ON u.id = o.user_id
            WHERE d.agent_id = @agentId
            ORDER BY o.CreatedAt DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@agentId", agentId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var orderId = reader["order_id"] != DBNull.Value ? Convert.ToInt32(reader["order_id"]) : 0;
                    var orderDate = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue;
                    var status = reader["status"]?.ToString() ?? "Unknown";
                    var customerName = reader["customer_name"]?.ToString() ?? "Unknown";

                    var order = new OrderHistoryDto
                    {
                        OrderId = orderId,
                        OrderDate = orderDate,
                        Status = status,
                        CustomerName = customerName,
                        Items = new List<OrderedItemDto>()
                    };

                    using (SqlConnection itemConn = SqlConn.GetConnection())
                    {
                        string itemQuery = @"
                    SELECT fi.item_name, fi.price, oi.quantity
                    FROM order_items oi
                    JOIN food_items fi ON fi.item_id = oi.item_id
                    WHERE oi.order_id = @orderId";

                        SqlCommand itemCmd = new SqlCommand(itemQuery, itemConn);
                        itemCmd.Parameters.AddWithValue("@orderId", orderId);
                        SqlDataReader itemReader = itemCmd.ExecuteReader();

                        while (itemReader.Read())
                        {
                            var itemName = itemReader["item_name"]?.ToString() ?? "Unknown";
                            var price = itemReader["price"] != DBNull.Value ? Convert.ToDecimal(itemReader["price"]) : 0;
                            var quantity = itemReader["quantity"] != DBNull.Value ? Convert.ToInt32(itemReader["quantity"]) : 0;

                            var item = new OrderedItemDto
                            {
                                ItemName = itemName,
                                Price = price,
                                Quantity = quantity
                            };

                            order.Items.Add(item);
                        }
                    }

                    orders.Add(order);
                }
            }

            return orders;
        }



    }
}