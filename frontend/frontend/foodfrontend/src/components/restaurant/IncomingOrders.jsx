import React, { useEffect, useState } from 'react';

import './IncomingOrders.css';

const IncomingOrders = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const restaurantId = localStorage.getItem('userId');

  useEffect(() => {
    fetch(`https://localhost:7025/api/Restaurant/restaurant/${restaurantId}/incoming`,{
      headers:{
        Authorization:`Bearer ${localStorage.getItem('token')}`
      }
    })
      .then(res => res.json())
      .then(data => {
        console.log(data);
        setOrders(data);
        setLoading(false);
      })
      .catch(err => {
        console.error('Error fetching orders:', err);
        setLoading(false);
      });
  }, [restaurantId]);

const handleAccept = (id) => {
  console.log(id)
  fetch(`https://localhost:7025/api/Restaurant/order/${id}/status`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ status: 'Accepted' }),
  }).then(() => window.location.reload());
};
 const handleReject = (id) => {
  fetch(`https://localhost:7025/api/Restaurant/order/${id}/status`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ status: 'Rejected' }),
  }).then(() => window.location.reload());
};


  if (loading) return <p>Loading incoming orders...</p>;

  return (
    <div className="orders-container-res">
      <h2>Incoming Orders</h2>
      {orders.length === 0 ? (
        <p>No new orders at the moment.</p>
      ) : (
        <ul className="orders-list-res">
          {orders.map(order => (
            <li key={order.id} className="order-card-res">
              <p><strong>Customer:</strong> {order.customerName}</p>
             <p><strong>Items:</strong> {order.foodItems?.map(i => i.itemName).join(', ') || 'No items listed'}</p>


              <p><strong>Total:</strong> ₹{order.totalPrice}</p>
              <p><strong>Status:</strong> {order.status}</p>
              <div className="order-actions-res">
                <button onClick={() => {
                  
                  handleAccept(order.id)
                }
                }>Accept</button>
                <button onClick={() => handleReject(order.id)}>Reject</button>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default IncomingOrders;

