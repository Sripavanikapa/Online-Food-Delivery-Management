import React from 'react';
import { Outlet, NavLink } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { FaUser, FaChartLine, FaClipboardList, FaHamburger, FaLock } from 'react-icons/fa';
import './Layout.css'; 

const Layout = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    localStorage.removeItem('userId');
    navigate('/');
  };

  const navigation = [
    { name: 'Dashboard', path: '/layout/dashboard', icon: <FaChartLine /> },
    { name: 'Users', path: '/layout/usermanagement', icon: <FaUser /> },

    { name: 'Orders', path: '/layout/orders', icon: <FaClipboardList /> },
    { name: 'Food Categories', path: '/layout/foodcategories', icon: <FaHamburger /> },
  ];

  return (
    <div className="dashboard-container-admin">
  <aside className="sidebar-admin">
    <div className="logo-admin">FOODIE ADMIN</div>
    <nav className="nav-menu-admin">
      {navigation.map((item) => (
        <NavLink
          key={item.name}
          to={item.path}
          className={({ isActive }) => `nav-link-admin ${isActive ? 'active-admin' : ''}`}
        >
          {item.icon}
          <span>{item.name}</span>
        </NavLink>
      ))}

   
      <button onClick={handleLogout}>
        <FaLock />
        <span>Logout</span>
      </button>
    </nav>
  </aside>

  <main className="main-content-admin">
    <Outlet /> 
  </main>
</div>
  );
};

export default Layout;
