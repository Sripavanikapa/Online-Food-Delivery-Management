import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { PieChart, Pie, Cell, Tooltip, Legend } from 'recharts';

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#AA336A'];

const TopRestaurantsPieChart = () => {
  const [restaurants, setRestaurants] = useState([]);

 
  useEffect(() => {
  const token = localStorage.getItem('token');

  axios.get('https://localhost:7025/api/Admin/top5', {
    headers: {
      Authorization: `Bearer ${token}`
    }
  })
  .then(res => setRestaurants(res.data))
  .catch(err => console.error('Error fetching restaurant list:', err));
}, []);

  return (
    <div style={{ textAlign: 'center' }}>
      <h3>Top 5 Restaurants</h3>
      <PieChart width={400} height={400}>
        <Pie
          data={restaurants}
          dataKey="orders"   
          nameKey="name"     
          cx="50%"
          cy="50%"
          outerRadius={120}
          label
        >
          {restaurants.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
          ))}
        </Pie>
        <Tooltip />
        <Legend />
      </PieChart>
    </div>
  );
};

export default TopRestaurantsPieChart;