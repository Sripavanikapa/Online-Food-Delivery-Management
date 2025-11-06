import React, { useEffect, useState } from 'react';
import './RestaurantProfile.css';

const RestaurantProfile = () => {
  const [profile, setProfile] = useState(null);
  const [addressList, setAddressList] = useState([]);
  const [orderCount, setOrderCount] = useState(0);
  const [newAddress, setNewAddress] = useState('');
  const [showAddressForm, setShowAddressForm] = useState(false);

  const token = localStorage.getItem('token');
  const restaurantId = localStorage.getItem('userId');

 
  useEffect(() => {
    fetch('https://localhost:7025/api/Restaurant/me', {
      headers: { Authorization: `Bearer ${token}` }
    })
      .then(res => res.json())
      .then(data => {
        console.log("Fetched profile:", data);
        setProfile(data);
      })
      .catch(err => console.error('Failed to fetch profile:', err));
  }, [token]);

 
  useEffect(() => {
    if (!profile || !profile.restaurantId) return;

 
    fetch(`https://localhost:7025/api/Address/get/alladdress/users?userid=${profile.restaurantId}`, {
      headers: { Authorization: `Bearer ${token}` }
    })
      .then(res => res.json())
      .then(data => {
        console.log("Fetched Address:", data);
        setAddressList(data);
      })
      .catch(err => console.error('Failed to fetch address:', err));

  
    fetch(`https://localhost:7025/api/Restaurant/order-history/${restaurantId}`, {
      headers: { Authorization: `Bearer ${token}` }
    })
      .then(async res => {
        const contentType = res.headers.get('content-type');
        if (!res.ok) {
          const text = await res.text();
          throw new Error(`Order history fetch failed: ${text}`);
        }

        if (contentType && contentType.includes('application/json')) {
          const data = await res.json();
          const acceptedOrders = data.filter(order => order.status === "Delivered");
          setOrderCount(acceptedOrders.length);
        } else {
          const text = await res.text();
          console.warn("Non-JSON response:", text);
          setOrderCount(0);
        }
      })
      .catch(err => console.error('Failed to fetch order history:', err));
  }, [token, restaurantId, profile]);


  const addAddress = async () => {
  if (!profile.restaurantId || !newAddress.trim()) {
    alert('Restaurant ID and address are required.');
    return;
  }

  const payload = {
    cust_id: profile.restaurantId,
    Address1: newAddress.trim(),
    Address2: "N/A"
  };

  try {
    const response = await fetch('https://localhost:7025/api/Address/create/address', {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(payload)
    });

    const contentType = response.headers.get('content-type');

    if (!response.ok) {
      if (contentType && contentType.includes('application/json')) {
        const errorData = await response.json();
        const errorMessages = Object.entries(errorData.errors || {})
          .map(([field, messages]) => `${field}: ${messages.join(', ')}`)
          .join('\n');
        alert(`Validation Error:\n${errorMessages}`);
      } else {
        const text = await response.text();
        alert(`Error: ${text}`);
      }
      return;
    }

    alert('Address added successfully!');
    setNewAddress('');
    setShowAddressForm(false);


    const res = await fetch(`https://localhost:7025/api/Address/get/addresses/users?userid=${profile.restaurantId}`, {
      headers: { Authorization: `Bearer ${token}` }
    });

    const refreshType = res.headers.get('content-type');
    if (refreshType && refreshType.includes('application/json')) {
      const data = await res.json();
      setAddressList(data);
    } else {
      console.warn("Address list response was not JSON");
    }

  } catch (error) {
    console.error('Add address error:', error);
    alert('Something went wrong while adding address.');
  }
};

  const shouldShowAddButton = addressList.length < 2;

  if (!profile) return <div>Loading profile...</div>;

  return (
    <div className="restaurant-profile">
      <div className="profile-header">
        <h3>{profile.ownerName}</h3>
        <p><strong>Phone:</strong> {profile.phoneno}</p>
        <p><strong>Status:</strong> {profile.status ? 'Active' : 'Inactive'}</p>
        <p><strong>Verification:</strong> {profile.isValid ? 'Verified Partner' : 'Unverified'}</p>
      </div>

      <div className="address-section">
        <h3>Address</h3>
        {addressList.length > 0 ? (
          addressList.map((addr, index) => (
            <p key={addr.addressId}><strong>Address {index + 1}:</strong> {addr.address}</p>
          ))
        ) : (
          <p>No address found.</p>
        )}

        {shouldShowAddButton && !showAddressForm && (
          <button onClick={() => setShowAddressForm(true)}>Add Address</button>
        )}

        {showAddressForm && (
          <div className="add-address-form">
            <input
              type="text"
              placeholder="Enter new address"
              value={newAddress}
              onChange={(e) => setNewAddress(e.target.value)}
            />
            <button onClick={addAddress}>Submit Address</button>
          </div>
        )}
      </div>

      <div className="profile-stats">
        <h3>Operational Info</h3>
        <p><strong>Orders Served:</strong> {orderCount}</p>
        <p><strong>Menu:</strong> <a href="/restaurant/ManageMenu">View Menu</a></p>
        <p><strong>Order History:</strong> <a href="/restaurant/OrderHistory">View History</a></p>
      </div>
    </div>
  );
};

export default RestaurantProfile;
