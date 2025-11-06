import React, { useEffect, useState } from 'react';
import './OrderHistory.css'; 

const OrderHistory = () => {
  const [history, setHistory] = useState([]);
  const restaurantId = localStorage.getItem('userId');
  const token = localStorage.getItem('token');

  useEffect(() => {
    fetch(`https://localhost:7025/api/Restaurant/order-history/${restaurantId}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
      .then(res => res.json())
      .then(data => setHistory(data))
      .catch(err => console.error('Failed to fetch order history:', err));
  }, [restaurantId, token]);

  return (
    <div className="order-history-page-res">
      <h2 style={{marginLeft:"10px"}}>Order History</h2>
      {history.length === 0 ? (
        <p>No orders found.</p>
      ) : (
        history.map(order => (
          <div key={order.orderId} className="order-card-res">
            <h6 style={{color:"darkOrange"}}>Order {order.orderId}</h6>
            <p><strong>Customer:</strong> {order.customerName}</p>
            <p>
  <strong>Status:</strong>{' '}
  <span style={{ color: order.status === 'Accepted' ? 'green' : 'red' }}>
    {order.status}
  </span>
</p>

            <p><strong>Date:</strong> {new Date(order.orderDate).toLocaleString()}</p>
            <ul>
              {order.items.map((item, index) => (
                <li key={index}>
                  {item.itemName} <br/>
                 Price : {item.price}/-<br/>
                  ItemQuantity : {item.quantity} 
                  <br/>
                  Total Cost : {item.totalItemPrice}/-
                </li>
              ))}
            </ul>
          </div>
        ))
      )}
    </div>
  );
};

export default OrderHistory;
