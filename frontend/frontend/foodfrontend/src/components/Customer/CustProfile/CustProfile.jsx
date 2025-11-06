import React, { useState, useEffect } from 'react';
import './CustProfile.css';
import { useNavigate } from 'react-router-dom';
import CustSearchBarNavbar from '../CustSearchBar/CustSearchBarNavbar';
 
const CustProfile = () => {
  const [user, setUser] = useState({ name: '', phone: '' });
  const [isEditing, setIsEditing] = useState(false);
  const [isLoggedIn, setIsLoggedIn] = useState(true);
 
  const navigate = useNavigate();
  useEffect(() => {
    const userId = localStorage.getItem('userId');
    const token = localStorage.getItem('token');
 
    if (userId && token) {
      fetch(`https://localhost:7025/api/User/get/userInfoById?userid=${userId}`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Accept': 'application/json'
        }
      })
        .then(res => res.json())
        .then(data => {
          setUser({
            name: data.name,
            phone: data.phno
          });
        })
        .catch(err => {
          console.error('Error fetching user info:', err);
          setIsLoggedIn(false);
        });
    } else {
      setIsLoggedIn(false);
    }
  }, []);
 
  const handleUpdate = () => setIsEditing(true);
 
  const handleSave = async () => {
    setIsEditing(false);
    try {
      const response = await fetch('https://localhost:7025/api/User/update/user', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
          'Accept': 'application/json'
        },
        body: JSON.stringify({
          name: user.name,
          phoneno: user.phone
        })
      });
 
      if (response.ok) {
        const updatedUser = await response.json();
        setUser({
          name: updatedUser.name,
          phone: updatedUser.phoneno
        });
        alert('User updated successfully!');
      } else {
        alert('Failed to update user.');
      }
    } catch (error) {
      console.error('Error updating user:', error);
      alert('Something went wrong while updating.');
    }
  };
 
  const handleLogout = () => {
    localStorage.clear();
    setIsLoggedIn(false);
    navigate('/');
  };
 
  const handleLogin = () => {
    setIsLoggedIn(true);
   
  };
 
  return (
    <div>
      <div className='profile-navbar'>
        <CustSearchBarNavbar></CustSearchBarNavbar>
      </div>
    <div className="user-profile">
     
      <h2>User Profile</h2>
      {isLoggedIn ? (
        <>
          <div className="input-row">
            <label>Name:</label>
            <input
              type="text"
              value={user.name}
              readOnly={!isEditing}
              onChange={(e) => setUser({ ...user, name: e.target.value })}
            />
          </div>
 
          <div className="input-row">
            <label>Phone:</label>
            <input
              type="text"
              value={user.phone}
              readOnly={!isEditing}
              onChange={(e) => setUser({ ...user, phone: e.target.value })}
            />
          </div>
 
          {isEditing ? (
            <button onClick={handleSave}>Save</button>
          ) : (
            <button onClick={handleUpdate}>Edit</button>
          )}
          <button onClick={handleLogout}>Logout</button>
        </>
      ) : (
        <button onClick={handleLogin}>Login</button>
      )}
    </div>
    </div>
  );
};
 
export default CustProfile;
 