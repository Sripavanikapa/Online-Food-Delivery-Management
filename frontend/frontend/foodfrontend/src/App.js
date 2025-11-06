import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './components/Login/LoginPage';
import MainRestaurant from '../src/components/restaurant/MainRestaurant.jsx'
import RegisterPage from './components/register/RegisterPage';
import CustProfile from './components/Customer/CustProfile/CustProfile';
import UpdateRestaurantStatus from './components/restaurant/UpdateRestaurantStatus';
import CustHome  from './components/Customer/CustHome';
import Landing from './components/Landing/Landing';
import Cart from './components/Customer/Cart/Cart'
import CustSearchBar from './components/Customer/CustSearchBar/CustSearchBar';
import FoodByCategory from './components/Customer/FoodByCategory/FoodItemsByCategory.jsx';
import PrivateRoute from './components/PrivateRoute';
import CustOrders from './components/Customer/CustGetOrders/CustOrders'
import OrderNow from './components/Customer/OrderNow/OrderNow';
import Layout from './components/Admin/Layout/Layout';
import Dashboard from './components/Admin/Dashboard/Dashboard';
import UserManagement from './components/Admin/UserManagement/UserManagement';
import FoodCategories from './components/Admin/FoodCategories/FoodCategories';
import Orders from './components/Admin/Orders/Orders';
import AgentHistory from './components/Agent/AgentHistory.jsx';
import MainAgent from './components/Agent/MainAgent.jsx';
import ActiveOrders from './components/Agent/ActiveOrders.jsx';
import Profile from './components/Agent/Profile.jsx';
import Sidebar from './components/Agent/Sidebar.jsx';
import CustCategory from './components/Customer/CustCategory/CustCategory.jsx'
import SpaStatus from './components/spa/SpaStatus.jsx';
import RestaurantCategory from './components/restaurant/RestaurantCategory.jsx';
import ForgotPassword from './components/ForgotPassword.jsx';

function App() {
  return (
   
    <Router>
      <SpaStatus />
      <Routes>
       
        <Route path="/" element={<Landing />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
          

        <Route path="/login" element={<LoginPage />} />
        <Route path="/restaurant/*" element={<MainRestaurant />} />
        <Route path="/profile" element={<PrivateRoute><CustProfile setActiveSection={() => {}}/></PrivateRoute>}/>
         <Route path="/register" element={<RegisterPage />} />
         <Route path="/customer" element={<CustHome />} />
          <Route path='/CustCategory' element={<CustCategory />}/>
         <Route path="/fooditems/results" element={<CustSearchBar />} />
         <Route path="/fooditems/category" element={<FoodByCategory />} />
         <Route path="/ordernow" element={<PrivateRoute><OrderNow /></PrivateRoute>} />
         <Route path="/orders" element={<PrivateRoute><CustOrders /></PrivateRoute>} />
         <Route path="/cart" element={<Cart/>}></Route>
         <Route path="/layout" element={<Layout />}>
            <Route index element={<Dashboard />} />
            <Route path="dashboard" element={<Dashboard />} />
            <Route path="usermanagement" element={<UserManagement />} />
            <Route path="foodcategories" element={<FoodCategories />} />
            <Route path="orders" element={<Orders />} />
         </Route>


         <Route
            path="/agent/*"
            element={
              <div className="main-layout">
                <Sidebar />
                <Routes>
                  <Route path="profile" element={<Profile />} />
                  <Route path="order-history" element={<AgentHistory />} />
                  <Route path="active-orders" element={<ActiveOrders />} />
                  <Route path="" element={<MainAgent />} />
                </Routes>
              </div>
            }
          />
 
   
      </Routes>
    </Router>
   
  );
}

export default App;
