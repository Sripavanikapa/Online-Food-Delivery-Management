import React, { useState, useEffect } from 'react';
import './RestaurantStatusUpdate.css';

function UpdateRestaurantStatus() {
  const [status, setStatus] = useState(null);
  const restaurantId = localStorage.getItem('userId');

  useEffect(() => {
    fetch(`https://localhost:7025/api/Restaurant/${restaurantId}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`
      }
    })
      .then(res => res.json())
      .then(data => {
        setStatus(data.status);
      })
      .catch(err => {
        console.error('Error fetching status:', err);
      });
  }, [restaurantId]);

  const toggleStatus = () => {
    const newStatus = !status;

    fetch(`https://localhost:7025/api/Restaurant/${restaurantId}/status`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify({ status: newStatus })
    })
      .then(res => {
        if (!res.ok) throw new Error('Failed to update status');
        return res.json();
      })
      .then(() => {
        setStatus(newStatus);
        alert(`Restaurant is now ${newStatus ? 'Open' : 'Closed'}`);
      })
      .catch(err => {
        console.error('Error updating status:', err);
        alert('Something went wrong');
      });
  };

  return (
    <div className="status-container">
      <h2>Restaurant Availability</h2>
      <p>Current Status: <strong>{status ? 'Open' : 'Closed'}</strong></p>
      <button onClick={toggleStatus}>
        {status ? 'Mark as Closed' : 'Mark as Open'}
      </button>
    </div>
  );
}

export default UpdateRestaurantStatus;
