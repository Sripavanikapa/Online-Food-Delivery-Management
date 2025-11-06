// import React, { useEffect, useState } from 'react';
// import { Pie, Bar, Line } from 'react-chartjs-2';
// import {
//   Chart as ChartJS,
//   ArcElement,
//   Tooltip,
//   Legend,
//   CategoryScale,
//   LinearScale,
//   BarElement,
//   PointElement,
//   LineElement,
//   Title
// } from 'chart.js';

// ChartJS.register(
//   ArcElement,
//   Tooltip,
//   Legend,
//   CategoryScale,
//   LinearScale,
//   BarElement,
//   PointElement,
//   LineElement,
//   Title
// );

// const RestaurantReports = () => {
//   const [orders, setOrders] = useState([]);
//   const [loading, setLoading] = useState(true);
//   const restaurantId = localStorage.getItem('userId');
//   const token = localStorage.getItem('token');
//   useEffect(() => {
//     fetch(`https://localhost:7025/api/Restaurant/order-history/${restaurantId}`, {
//       headers: {
//         Authorization: `Bearer ${token}`
//       }
//     })
//       .then(res => res.json())
//       .then(data => {
//         if (Array.isArray(data)) {
//           setOrders(data);
//         } else {
//           setOrders([]);
//         }
//         setLoading(false);
//       })
//       .catch(err => {
//         console.error('Failed to fetch order history:', err);
//         setOrders([]);
//         setLoading(false);
//       });
//   }, [restaurantId]);

//   if (loading) {
//     return <div>Loading reports...</div>;
//   }

//   if (!orders.length) {
//     return <div>No order history available.</div>;
//   }

//   // Pie Chart: Order Status Distribution
//   const statusCounts = orders.reduce((acc, order) => {
//     acc[order.status] = (acc[order.status] || 0) + 1;
//     return acc;
//   }, {});
//   const pieData = {
//     labels: Object.keys(statusCounts),
//     datasets: [{
//       data: Object.values(statusCounts),
//       backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0'],
//     }]
//   };

//   // Bar Chart: Item Sales
//   const itemSales = {};
//   orders.forEach(order => {
//     if (Array.isArray(order.items)) {
//       order.items.forEach(item => {
//         const key = item.itemName;
//         const total = item.quantity * item.price;
//         itemSales[key] = (itemSales[key] || 0) + total;
//       });
//     }
//   });
//   const barData = {
//     labels: Object.keys(itemSales),
//     datasets: [{
//       label: 'Total Sales (₹)',
//       data: Object.values(itemSales),
//       backgroundColor: '#4BC0C0',
//     }]
//   };

//   // Line Chart: Orders Per Day
//   const dailyOrders = {};
//   orders.forEach(order => {
//     const date = new Date(order.orderDate).toLocaleDateString();
//     dailyOrders[date] = (dailyOrders[date] || 0) + 1;
//   });
//   const lineData = {
//     labels: Object.keys(dailyOrders),
//     datasets: [{
//       label: 'Orders per Day',
//       data: Object.values(dailyOrders),
//       fill: false,
//       borderColor: '#36A2EB',
//       tension: 0.1
//     }]
//   };

//   return (
//     <div style={{ padding: '20px' }}>
//       <h2>📊 Order Status Overview</h2>
//       <Pie data={pieData} />

//       <h2 style={{ marginTop: '40px' }}>💰 Menu Item Sales</h2>
//       <Bar data={barData} />

//       <h2 style={{ marginTop: '40px' }}>📅 Daily Orders</h2>
//       <Line data={lineData} />
//     </div>
//   );
// };

// export default RestaurantReports;
