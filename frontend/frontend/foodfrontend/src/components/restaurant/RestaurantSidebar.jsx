import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import './RestaurantSidebar.css';
import reports from '../../assets/reports.png';
import profile from '../../assets/profile.png';
import IncomingOrders from '../../assets/IncomingOrders.png';
import MenuManager from '../../assets/MenuManager.png';
import status from '../../assets/status.png';
import history from '../../assets/History.png';
import restaurantLogo from '../../assets/restaurant icon.png';
import burger from '../../assets/mdi_food.png';

const RestaurantSidebar = () => {
  const [restaurantName, setRestaurantName] = useState('Loading...');
  
  useEffect(() => {
    fetch('https://localhost:7025/api/Restaurant/me', {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`
      }
    })
      .then(res => res.json())
      .then(data => {
        setRestaurantName(data.ownerName);
      })
      .catch(err => {
        console.error('Failed to fetch restaurant info:', err);
        setRestaurantName('Unknown');
      });
  }, []);

  return (
    <div className="sidebar">
      <h2 className="sidebar-title">
        <Link to="/"><img src={restaurantLogo} alt="restaurantLogo" className="sidebar-icon" style={{ height: "32px", width: "32px" ,marginLeft:"-20px" }} /></Link>
        {restaurantName} 
      </h2>
      <nav>
        <ul className="sidebar-nav">
        
          <li><Link to="/restaurant/RestaurantProfile" className="sidebar-link"><img src={profile} alt="profile" className="sidebar-icon" />Profile</Link></li>
          <li><Link to="/restaurant/IncomingOrders" className="sidebar-link"><img src={IncomingOrders} alt="IncomingOrders" className="sidebar-icon" />Incoming Orders</Link></li>
          <li><Link to="/restaurant/RestaurantCategory" className="sidebar-link"><img src={burger} alt="burger" className="sidebar-icon" />List of Category</Link></li>
          <li><Link to="/restaurant/ManageMenu" className="sidebar-link"><img src={MenuManager} alt="MenuManager" className="sidebar-icon" />Menu Manager</Link></li>
          <li><Link to="/restaurant/UpdateRestaurantStatus" className="sidebar-link"><img src={status} alt="status" className="sidebar-icon" />Update Status</Link></li>
          <li><Link to="/restaurant/OrderHistory" className="sidebar-link"><img src={history} alt="history" className="sidebar-icon" />Order History</Link></li>
        </ul>
      </nav>
    </div>
  );
};

export default RestaurantSidebar;
