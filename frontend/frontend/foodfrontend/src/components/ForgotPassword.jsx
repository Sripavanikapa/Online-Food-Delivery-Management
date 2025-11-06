import React, { useState } from 'react';
import './ForgotPassword.css'; // optional styling
import { useNavigate } from 'react-router-dom';

const ForgotPassword = ({ setActiveSection }) => {
  const [phoneno, setPhoneno] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const navigate = useNavigate();

  const handleReset = async () => {
    if (!phoneno.trim() || !newPassword.trim()) {
      alert("Phone number and new password are required.");
      return;
    }

    try {
      const response = await fetch('https://localhost:7025/api/User/forgot-password', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ phoneno, newPassword })
      });

      const data = await response.json();

      if (response.ok) {
        alert(data.message);
        setActiveSection('login');
      } else {
        alert(data.message || "Failed to reset password.");
      }
    } catch (error) {
      console.error("Error resetting password:", error);
      alert("Something went wrong. Please try again.");
    }
  };

  return (
    <div className="forgot-container">
      <h2>Reset Your Password</h2>
      <input
        type="text"
        placeholder="Phone Number"
        value={phoneno}
        onChange={(e) => setPhoneno(e.target.value)}
      />
      <input
        type="password"
        placeholder="New Password"
        value={newPassword}
        onChange={(e) => setNewPassword(e.target.value)}
      />
      <button onClick={handleReset}>Reset Password</button>
    </div>
  );
};

export default ForgotPassword;
