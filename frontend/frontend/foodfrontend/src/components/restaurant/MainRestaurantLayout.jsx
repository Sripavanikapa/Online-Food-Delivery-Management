import React from 'react';
import RestaurantSidebar from './RestaurantSidebar';
import { Outlet } from 'react-router-dom';

const MainRestaurantLayout = () => {
  return (
    <div style={{ display: 'flex' }}>
      <RestaurantSidebar />
      <div style={{ flex: 1, padding: '20px' }}>
        <Outlet /> 
      </div>
    </div>
  );
};

export default MainRestaurantLayout;
