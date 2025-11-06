import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { FaUsersCog } from 'react-icons/fa';
import './UserManagement.css';

const UserRow = ({ user, activeTab, onApprove, onDelete }) => {
  const handleApproveClick = () => {
    const id = activeTab === 'Restaurant' ? user.restaurantId
      : activeTab === 'Delivery Agent' ? user.agentId
      : user.id;
    onApprove(id, true);
  };

  const handleDeleteClick = () => {
    const phoneNo = user.phoneno;
    onDelete(phoneNo);
  };

  const actionButtons = (
    <td>
      <button
        className="btn-action-admin"
        onClick={handleApproveClick}
        disabled={user.isActive === false || user.isValid === true}
      >
        Approve
      </button>
      <button
        className="btn-action-admin btn-delete-admin"
        onClick={handleDeleteClick}
        disabled={user.isActive === false}
      >
        Delete
      </button>
    </td>
  );

  return (
    
 <tr>
    <td>{activeTab === 'Restaurant' ? user.restaurantId : activeTab === 'Delivery Agent' ? user.agentId : user.id}</td>
    <td>{activeTab === 'Restaurant' ? user.ownerName || 'N/A' : activeTab === 'Delivery Agent' ? user.agentName || 'N/A' : user.name}</td>

    <td>{user.phoneno}</td>
    {activeTab !== 'Customer' && (
      <td>
        <span className={`status-badge-admin ${user.isActive === false ? 'deleted-admin' : user.isValid ? 'active-admin' : 'inactive-admin'}`}>
          {user.isActive === false ? 'Deleted' : user.isValid ? 'Approved' : 'Pending'}
        </span>
      </td>
    )}
    {activeTab !== 'Customer' && actionButtons}
  </tr>

  );
};

const getTableHeaders = (activeTab) => {
  if (activeTab === 'Restaurant') return ['ID', 'Restaurant Name', 'Phone', 'Status', 'Actions'];
  if (activeTab === 'Delivery Agent') return ['ID', 'Name', 'PhoneNo', 'Status', 'Actions'];
  if (activeTab === 'Customer') return ['ID', 'Name', 'Phone No'];
  return [];
};

const UserManagement = () => {
  const [activeTab, setActiveTab] = useState('Restaurant');
  const [restaurantUsers, setRestaurantUsers] = useState([]);
  const [deliveryUsers, setDeliveryUsers] = useState([]);
  const [customerUsers, setCustomerUsers] = useState([]);

  useEffect(() => {
    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    const data1=axios.get('https://localhost:7025/getRestaurants', { headers })
      .then(res =>
       
         setRestaurantUsers(res.data)
        
        )
         
      .catch(err => console.error('Error fetching restaurant users:', err));
console.log(data1);
    axios.get('https://localhost:7025/api/Admin/getAllDeliveryAgents', { headers })
      .then(res => setDeliveryUsers(res.data))
      .catch(err => console.error('Error fetching delivery agents:', err));

    axios.get('https://localhost:7025/getCustomers', { headers })
      .then(res => setCustomerUsers(res.data))
      .catch(err => console.error('Error fetching customers:', err));
  }, []);

  const handleApproveClick = async (id, isValid) => {
    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    try {
      const res = await axios.put(`https://localhost:7025/api/Admin/${id}/${isValid}/approve`, {}, { headers });
      alert(res.data?.message || 'User approved successfully');

      if (activeTab === 'Restaurant') {
        setRestaurantUsers(prev => prev.map(u => u.restaurantId === id ? { ...u, isValid: true } : u));
      } else if (activeTab === 'Delivery Agent') {
        setDeliveryUsers(prev => prev.map(u => u.agentId === id ? { ...u, isValid: true } : u));
      }
    } catch (err) {
      console.error('Error approving user:', err);
    }
  };

  const handleDeleteClick = async (phoneNo) => {
    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    try {
      const res = await axios.delete(`https://localhost:7025/api/Admin/deleteUser`, {
        headers,
        params: { phoneNo }
      });

      alert(res.data?.message || 'User deleted successfully');

      if (activeTab === 'Restaurant') {
        setRestaurantUsers(prev => prev.map(u => u.phoneno === phoneNo ? { ...u, isActive: false } : u));
      } else if (activeTab === 'Delivery Agent') {
        setDeliveryUsers(prev => prev.map(u => u.phoneno === phoneNo ? { ...u, isActive: false } : u));
      } else {
        setCustomerUsers(prev => prev.map(u => u.phoneno === phoneNo ? { ...u, isActive: false } : u));
      }
    } catch (err) {
      console.error('Error deleting user:', err);
    }
  };

  const getActiveUsers = () => {
    if (activeTab === 'Restaurant') return restaurantUsers;
    if (activeTab === 'Delivery Agent') return deliveryUsers;
    return customerUsers;
  };

  const activeUsers = getActiveUsers();
  const tableHeaders = getTableHeaders(activeTab);

  return (
    <div className="section-card-admin">
      <h2><FaUsersCog style={{ marginRight: '10px' }} />User Management</h2>

      <div className="tabs-admin">
        <ul className="tab-list-admin">
          <li className={`tab-item-admin ${activeTab === 'Restaurant' ? 'active' : ''}`} onClick={() => setActiveTab('Restaurant')}>Restaurants</li>
          <li className={`tab-item-admin ${activeTab === 'Delivery Agent' ? 'active' : ''}`} onClick={() => setActiveTab('Delivery Agent')}>Delivery Agents</li>
          <li className={`tab-item-admin ${activeTab === 'Customer' ? 'active' : ''}`} onClick={() => setActiveTab('Customer')}>Customers</li>
        </ul>
      </div>

      <table className="data-table-admin">
        <thead>
          <tr>
            {tableHeaders.map(header => (
              <th key={header}>{header}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {activeUsers.map((user, index) => (
            <UserRow
              key={user.id || user.agentId || user.restaurantId || index}
              user={user}
              activeTab={activeTab}
              onApprove={handleApproveClick}
              onDelete={handleDeleteClick}
            />
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default UserManagement;