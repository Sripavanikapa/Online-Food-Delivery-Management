import React, { useState, useEffect } from 'react';
import './Dashboard.css';
import { FaUser, FaStore, FaClipboardList } from 'react-icons/fa';
import PieChart from '../PieChart/PieChart';
import axios from 'axios';

const QuickStatCard = ({ title, value, icon, color }) => (
  <div className={`stat-card-admin ${color}`}>
    <div className="stat-icon-admin">{icon}</div>
    <div className="stat-info-admin">
      <p className="stat-title-admin">{title}</p>
      <h3 className="stat-value-admin">{value}</h3>
    </div>
  </div>
);

const Dashboard = () => {
  const [userCount, setUserCount] = useState(0);
  const [restaurantCount, setRestaurantCount] = useState(0);
  const [orderCount, setOrderCount] = useState(0);
  const [topRestaurants, setTopRestaurants] = useState([]);

  useEffect(() => {
    const token = localStorage.getItem('token');
    const headers = { Authorization: `Bearer ${token}` };

    axios.get('https://localhost:7025/TotalUsers', { headers })
      .then(res => setUserCount(res.data))
      .catch(err => console.error('Error fetching user count:', err));

    axios.get('https://localhost:7025/TotalRestaurants', { headers })
      .then(res => setRestaurantCount(res.data))
      .catch(err => console.error('Error fetching restaurant count:', err));

    axios.get('https://localhost:7025/TotalOrders', { headers })
      .then(res => setOrderCount(res.data))
      .catch(err => console.error('Error fetching order count:', err));

    axios.get('https://localhost:7025/api/Admin/top5', { headers })
      .then(res => setTopRestaurants(res.data))
      .catch(err => console.error('Error fetching top restaurants:', err));
  }, []);

  return (
    <section className="dashboard-sections-admin">
  <div className="section-overview-admin">
    <h1 align="left">Admin Dashboard</h1>
    <div className="stats-grid-admin">
      <QuickStatCard title="Total Users" value={userCount.toLocaleString()} icon={<FaUser />} color="blue" />
      <QuickStatCard title="Total Restaurants" value={restaurantCount.toLocaleString()} icon={<FaStore />} color="orange" />
      <QuickStatCard title="Total Orders" value={orderCount.toLocaleString()} icon={<FaClipboardList />} color="green" />
    </div>

    <div className="charts-grid-admin">
      <div className="chart-widget-admin">
        <PieChart />
      </div>
      <div className="chart-widget-admin top-restaurants-admin">
        <ul>
          {topRestaurants.length > 0 ? (
            topRestaurants.map((rest, index) => (
              <li key={index}>
                <span>{rest.name}</span>
                
              </li>
            ))
          ) : (
            <p>No data available</p>
          )}
        </ul>
      </div>
    </div>
  </div>
</section>
  );
};

export default Dashboard;