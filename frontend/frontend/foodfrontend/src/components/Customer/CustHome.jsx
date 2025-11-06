import React, { useState } from 'react';
import CustNavbar from './CustNavbar/CustNavbar';
import CustAnimation from './CustAnimation/CustAnimation';
import CustBackground from './CustBackground/CustBackground';
import CustServices from './CustServices/CustServices';
import CustFooter from './CustFooter/CustFooter';
import CustCategory from './CustCategory/CustCategory';
import CustBrands from './CustBrands/CustBrands';
import CustOrders from './CustGetOrders/CustOrders';

import './CustHome.css';

function CustHome() {
  const [activeSection, setActiveSection] = useState('home');

  return (
    <div className="App">
      <div className="navbar-div">
        <CustNavbar setActiveSection={setActiveSection} />
      </div>

      {activeSection === 'home' && (
        <>
          <div className="background-div">
            <CustBackground />
          </div>
          <div className="Animation-div">
            <CustAnimation />
          </div>
        </>
      )}

      {activeSection === 'category' && (
        <div className="Category-div">
          <CustCategory />
        </div>
      )}

      {activeSection === 'brands' && (
        <div className="brands-div">
          <CustBrands />
        </div>
      )}

      {activeSection === 'services' && (
        <div className="custservices-div">
          <CustServices />
        </div>
      )}

      {activeSection === 'contact' && (
        <div className="custFooter-div">
          <CustFooter />
        </div>
      )}
      {activeSection === 'orders' && (
  <div className="orders-div">
    <CustOrders />
  </div>
)}
    </div>
  );
}

export default CustHome;
