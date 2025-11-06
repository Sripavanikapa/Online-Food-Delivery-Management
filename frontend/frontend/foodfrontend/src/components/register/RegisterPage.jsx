import React, { useState } from 'react';
import './RegisterPage.css';
import RegisterImage from '../../assets/Register.png'; 
import { Link } from 'react-router-dom';
const RegisterPage = ({ setActiveSection }) => {
  const [name, setName] = useState('');
  const [phoneno, setPhoneno] = useState('');
  const [password, setPassword] = useState('');
  const [role, setRole] = useState('--select--');

  const handleRegister = async () => {
      if (!name.trim()) {
    alert('Name is required');
    return;
  }

  if (!phoneno.trim()) {
    alert('Phone number is required');
    return;
  }

  if (!password.trim()) {
    alert('Password is required');
    return;
  }

  if (role === '--select--') {
    alert('Please select a valid role');
    return;
  }
    try {
      const response = await fetch('https://localhost:7025/api/User/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name:name, phoneno:phoneno, password:password, role:role })
      });


     

      const data = await response.json();

      if (response.ok) {
        alert('Registration successful!');
        window.location.href = '/login';
      } else {
    alert(data.message || 'Registration failed');
  }
    } catch {
    }
  };

  return (
    <div className="register-container">
  
      <div className="register-image">
        <img src={RegisterImage} alt="Register visual" className="register-img" />
      </div>

 
      <div className="register-form">
        <h2>Get Started Now</h2>
        <input type="text" placeholder="Name" value={name} onChange={(e) => setName(e.target.value)} />
        <input type="text" placeholder="Phone Number" value={phoneno} onChange={(e) => setPhoneno(e.target.value)} />
        <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
        
        <select value={role} onChange={(e) => setRole(e.target.value)}>
         <option value="--select--" disabled>-- Select Role --</option>
          <option value="restaurant">Restaurant</option>
          <option value="admin">Admin</option>
          <option value="customer">Customer</option>
          <option value="deliveryagent">DeliveryAgent</option>
        </select>

        <button onClick={handleRegister}>Register</button>
      <p className="signin-link">
  Have an account?{' '}
  <a onClick={() => setActiveSection('login')}>Sign in</a>
</p>
      </div>
    </div>
  );
};

export default RegisterPage;
