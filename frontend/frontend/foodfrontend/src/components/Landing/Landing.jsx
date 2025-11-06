import React, { useState } from 'react';
import heroImage from '../../assets/heroImage.png';
import './Landing.css'; 
import { useNavigate } from 'react-router-dom';
import service1 from '../../assets/service1.png';
import service2 from '../../assets/service2.png';
import service3 from '../../assets/service2.png';
import CustFooter from '../Customer/CustFooter/CustFooter';
import CustServices from '../Customer/CustServices/CustServices';
import CustCategory from '../Customer/CustCategory/CustCategory';
import LoginPage from '../Login/LoginPage';
import RegisterPage from '../register/RegisterPage';
import ForgotPassword from '../ForgotPassword.jsx';
import CustHome from '../Customer/CustHome'; 

const Landing = () => {
  const Navigate = useNavigate();
  const [activeSection, setActiveSection] = useState('none');

const handleLoginClick = () => {
  setActiveSection('login');
};

  const handleCustomerClick = () => {
   setActiveSection('customer')
  };

  return (
    <div className="landing-container">
     
      <nav className="navbar">
        <div className="logo-landing">Foodeli</div>
        <ul className="nav-links">
          <li><button className="nav-links" onClick={() => setActiveSection('none')}>Home</button></li>
          <li><button className="nav-links" onClick={() => setActiveSection('why')}>Why Foodeli?</button></li>
          <li><button className="nav-links" onClick={() => setActiveSection('services')}>Services</button></li>
          <li><button className="nav-links" onClick={() => setActiveSection('category')}>Categories</button></li>
          <li><button className="nav-links" onClick={() => setActiveSection('contact')}>Contact</button></li>
        </ul>
        <button className="login-btn-landing" onClick={handleLoginClick}>Login</button>
      </nav>

      {activeSection === 'none' && (
        <section className="hero-section-landing">
          <div className="hero-text-landing">
            <h1>Claim Best Offer on Fast Food & Restaurants</h1>
            <p>Our job is to filling your tummy with delicious food and with fast and free delivery</p>
            <button className="cta-btn" onClick={handleCustomerClick}>Get Started</button>
          </div>
          <div className="hero-image">
            <img src={heroImage} alt="Delicious food" />
          </div>
        </section>
      )}

      
      {activeSection === 'why' && (
        <section className="hero-section-landing">
          <div className="hero-text-landing">
            <h2>Why Foodeli?</h2>
            <p>We connect hungry customers with amazing restaurants and fast delivery agents.</p>
            <ul>
              <li>Fast delivery</li>
              <li>Wide restaurant network</li>
              <li>Easy ordering experience</li>
            </ul>
          </div>
        </section>
      )}

     
      {activeSection === 'services' && (
        <section>
          <CustServices />
        </section>
      )}

      
      {activeSection === 'category' && (
        <section>
          <CustCategory />
        </section>
      )}

   
      {activeSection === 'contact' && (
        <section>
          <div id="footer-landing">
            <CustFooter />
          </div>
        </section>
      )}
      {activeSection === 'login' && (
  <section>
    <LoginPage setActiveSection={setActiveSection}/>
  </section>
)}
{activeSection === 'register' && (
  <section>
    <RegisterPage setActiveSection={setActiveSection} />
  </section>
)}
{activeSection === 'forgot' && (
  <section>
    <ForgotPassword setActiveSection={setActiveSection} />
  </section>
)}
{activeSection === 'customer' && (
  <section>
    <CustHome />
  </section>
)}

    </div>
  );
};

export default Landing;
