import React, { useEffect, useState } from 'react';
import './AgentHistory.css';

const AgentHistory = () => {
  const [history, setHistory] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const agentId = localStorage.getItem('userId');
  const token = localStorage.getItem('token');

  useEffect(() => {
    if (!agentId || !token) {
      setError('Missing agent ID or token');
      setLoading(false);
      return;
    }

    fetch(`https://localhost:7025/api/DeliveryAgent/Agentorder-history/${agentId}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
      .then(async res => {
        if (!res.ok) {
          const message = await res.text();
          throw new Error(`Server error: ${res.status} - ${message}`);
        }
        return res.json();
      })
      .then(data => {
        setHistory(data);
        setLoading(false);
      })
      .catch(err => {
        console.error('Failed to fetch order history:', err);
        setError('Failed to fetch order history');
        setLoading(false);
      });
  }, [agentId, token]);

  if (loading) return <p className="loading-text">Loading order history...</p>;
  if (error) return <p className="error-text">{error}</p>;

  return (
    <div className="order-history-page-res">
      <h2 className="history-heading">Order History</h2>
      {history.length === 0 ? (
        <p className="no-orders-text">No orders found.</p>
      ) : (
        history.map(order => (
          <div key={order.orderId} className="order-card-res">
            <h6 className="order-id">Order {order.orderId}</h6>
            <p><strong>Customer:</strong> {order.customerName || 'Unknown'}</p>
            <p>
              <strong>Status:</strong>{' '}
              <span style={{ color: order.status === 'Accepted' ? 'green' : 'red' }}>
                {order.status}
              </span>
            </p>
            <p><strong>Date:</strong> {new Date(order.orderDate).toLocaleString()}</p>

            {order.items && order.items.length > 0 ? (
              <ul className="item-list">
                {order.items.map((item, index) => (
                  <li key={index} className="item-entry">
                    <strong>{item.itemName}</strong><br />
                    Price: ₹{item.price}/-<br />
                    Quantity: {item.quantity}<br />
                    Total Cost: ₹{item.totalItemPrice || item.price * item.quantity}/-
                  </li>
                ))}
              </ul>
            ) : (
              <p className="no-items-text">No items found for this order.</p>
            )}
          </div>
        ))
      )}
    </div>
  );
};

export default AgentHistory;
