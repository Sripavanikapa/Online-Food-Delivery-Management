import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './LoginPage.css';
import LoginImage from '../../assets/LoginImage.png'; 
import MainRestaurant from '../restaurant/MainRestaurant';
import { Link } from 'react-router-dom'; 
const LoginPage = ({ setActiveSection }) => {
  const [phoneno, setPhoneno] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

const handleLogin = async () => {
 try {
    const response = await fetch(`https://localhost:7025/api/Auth/login?phoneno=${phoneno}&password=${password}`, {
      method: 'POST'
    });

    const data = await response.json();

    if (response.ok) {
      localStorage.setItem('token', data.accessToken);
      localStorage.setItem('role', data.role); 
      localStorage.setItem('userId', data.id); 

      alert('Login successful!');

    
      if (data.role === 'restaurant') {
         navigate('/restaurant/MainRestaurant');
      } else if (data.role === 'admin') {
        navigate('/layout');
      } 
      else if (data.role === 'customer') {
         setActiveSection('customer');
      } 
      else if (data.role === 'deliveryagent') {
        navigate('/agent');
      } 
      else {
        navigate('/'); 
      }
    } else {
      alert(data.message || 'Login failed');
    }
  } catch (error) {
    console.error('Login error:', error);
    alert('Something went wrong');
  }
};


  return (
    <div className="login-container">
   
      <div className="login-image">
        <img src={LoginImage} style={{height:"330px",width:"330px"}} alt="Login visual" className="login-img" />
      </div>

     
      <div className="login-form">
        <h2>Welcome back!</h2>
        <p>Enter your Credentials to access your account</p>
        <input
          type="text"
          placeholder="Enter your Phone Number"
          value={phoneno}
          onChange={(e) => setPhoneno(e.target.value)}
        />
        <input
          type="password"
          placeholder="Enter your password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
         
        <button onClick={handleLogin}>Login</button>
      <p>
  Not registered yet?
  <span className="link-button" onClick={() => setActiveSection('register')}>
    Register Now
  </span>
</p>

         <div className="forgot-password-link">
  <a onClick={() => setActiveSection('forgot')}>Forgot Password?</a>

  </div>
      </div>
    </div>
  );
};

export default LoginPage;
