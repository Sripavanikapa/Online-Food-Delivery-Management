import React from 'react';
import { Routes, Route } from 'react-router-dom';
import RestaurantSidebar from './RestaurantSidebar';
import RestaurantDashboard from './RestaurantDashboard';
import RestaurantProfile from './RestaurantProfile';
import ManageMenu from './ManageMenu';
import IncomingOrders from './IncomingOrders';
import OrderHistory from './OrderHistory';
import MainRestaurantLayout from './MainRestaurantLayout';
import UpdateRestaurantStatus from './UpdateRestaurantStatus';
import RestaurantReports from './RestaurantReports';
import RestaurantCategory from './RestaurantCategory';
function MainRestaurant() {
  return (
    <div style={{ display: 'flex' }}>
      <RestaurantSidebar />
      <div style={{ flex: 1, padding: '20px' }}>
        <Routes>
           <Route path="RestaurantCategory" element={<RestaurantCategory />} />
         < Route  path="/restaurant" element={<MainRestaurantLayout />}/>
          <Route path="dashboard" element={<RestaurantDashboard />} />
          <Route path="RestaurantProfile" element={<RestaurantProfile />} />
          <Route path="ManageMenu" element={<ManageMenu />} />
          <Route path="IncomingOrders" element={<IncomingOrders />} />
          <Route path="OrderHistory" element={<OrderHistory />} />
          <Route path="Reports" element={<RestaurantReports />} />
          <Route path="UpdateRestaurantStatus" element={<UpdateRestaurantStatus />} />
        </Routes>
      </div>
    </div>
  );
}

export default MainRestaurant;

