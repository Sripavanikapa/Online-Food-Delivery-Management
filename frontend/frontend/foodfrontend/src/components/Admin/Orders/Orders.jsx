import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Orders.css'; 
import {FaClipboardList} from 'react-icons/fa';

const Orders = () => {
  const [orders, setOrders] = useState([]);
  const [filter, setFilter] = useState('');

  const fetchOrders = async () => {
    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    try {
      const res = await axios.get('https://localhost:7025/api/Admin/AllOrders', { headers });
      setOrders(res.data);
    } catch (err) {
      console.error('Error fetching orders:', err);
    }
  };

 
  const assignAgent = async (orderId) => {
    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    try {
      const response = await axios.post(
        `https://localhost:7025/api/DeliveryAgentAssignment/assign?orderId=${orderId}`,
        null,
        { headers }
      );
      
   
      console.log(response.data.Message);
      alert("Delivery Agent Assigned Successfully!!");

      await fetchOrders();
    } catch (error) {
      console.error('Error assigning delivery agent:', error);
      
    }
  };

  useEffect(() => {
    fetchOrders();
  }, []);

  const filteredOrders = orders.filter(order =>
    order.restaurantName.toLowerCase().includes(filter.toLowerCase())
  );

  return (
    <div className='section-card-admin' style={{ padding: '20px' }}>
  <h2 align='left'><FaClipboardList style={{ marginRight: '10px' }} />Orders Dashboard</h2>

  <input
    type="text"
    placeholder="Filter by restaurant name"
    value={filter}
    onChange={(e) => setFilter(e.target.value)}
    className="filter-input-admin"
  />

  <table className="orders-table-admin" cellPadding="10">
    <thead>
      <tr>
        <th>Order ID</th>
        <th>Restaurant</th>
        <th>Customer</th>
        <th>Total Price</th>
        <th>Agent ID</th>
         <th>Agent Name</th>
      </tr>
    </thead>
    <tbody>
      {filteredOrders.map(order => (
        <tr key={order.id}>
          <td>{order.id}</td>
          <td>{order.restaurantName}</td>
          <td>{order.customerName}</td>
          <td>{order.totalPrice}</td>
          <td>
            {order.agentId ? (
              order.agentId
            ) : (
              <button className="assign-btn-admin" onClick={() => assignAgent(order.id)}>Assign Agent</button>
            )}
          </td>
          <td>{order.agentName}</td>
        </tr>
      ))}
    </tbody>
  </table>
</div>
  );
};

export default Orders;