import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './ActiveOrders.css';

function ActiveOrders() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const agentId = localStorage.getItem("userId");

  useEffect(() => {
    axios
      .get(`https://localhost:7025/api/Order/GetActiveOrders?id=${agentId}`)
      .then(response => {
        console.log(response.data);
        setOrders(response.data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching active orders:', error);
        setLoading(false);
      });
  }, [agentId]);

  const handleDelivered = async (orderId) => {
    try {
      const response = await axios.put(
        `https://localhost:7025/api/Order/UpdateStatusToDelivered?orderId=${orderId}`
      );
      if (response.status === 200) {
        console.log(response);
        setOrders(prev =>
          prev.map(order =>
            order.orderId === orderId ? { ...order, status: 'Delivered' } : order
          )
        );
        alert("Order marked as delivered.");
      }
    } catch (error) {
      console.error("Failed to update order status:", error);
      alert("Failed to update status.");
    }
  };

  if (loading) {
    return <p className="loading-text">Loading active orders...</p>;
  }

  return (
    <div className="active-orders-container">
      <h2 className="active-orders-heading">Active Orders</h2>
      {orders.length === 0 ? (
        <p className="no-orders-text">No active orders found.</p>
      ) : (
        <ul className="orders-list">
          {orders.map(order => (
            <li key={order.orderId} className="order-item">
              <strong>Order ID:</strong> {order.orderId}<br />
              <strong>Status:</strong> {order.status}<br />
              <strong>Restaurant:</strong> {order.restaurantName}<br />
              <strong>Restaurant Address:</strong> {order.restaurantAddress}<br />
              <strong>Customer:</strong> {order.customerName}<br />
              <strong>Customer Address:</strong> {order.customerAddress}<br />
              {order.items && order.items.length > 0 && (
                
                <ul className="item-list">
                  {order.items.map((item, index) => (
                    
                    <li key={index} className="item-entry">
                      <strong>{item.itemName}</strong><br />
                     <strong>Price: ₹</strong> {item.price}/-<br />
                    </li>
                  ))}
                </ul>
              )}
              <button
                className="delivered-button"
                onClick={() => handleDelivered(order.orderId)}
                disabled={order.status === 'Delivered'}
              >
                {order.status === 'Delivered' ? 'Delivered' : 'Mark as Delivered'}
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default ActiveOrders;
