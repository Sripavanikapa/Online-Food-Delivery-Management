import React, { useEffect, useState } from "react";
import "./Profile.css";

const Profile = () => {
  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");
  const [addresses, setAddresses] = useState([]);
  const [editing, setEditing] = useState(false);
  const [newAddress, setNewAddress] = useState("");
  const [addressId, setAddressId] = useState(null);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
  const [foodItem, setFoodItem] = useState(null);
 
  const [selectedAddressId, setSelectedAddressId] = useState('');
  const [orderPlaced, setOrderPlaced] = useState(false);
  const [showAddInput, setShowAddInput] = useState(false);

  const [quantity, setQuantity] = useState(1);
  const [cartCount, setCartCount] = useState(0);

  const userId = localStorage.getItem('userId');
 
  const token = localStorage.getItem("token");
  const agentId = localStorage.getItem("userId");

  const fetchAgentName = async () => {
    try {
      const res = await fetch(`https://localhost:7025/api/DeliveryAgent/GetAgentName?id=${agentId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      const data = await res.text();
      console.log(data);
      setName(data);
    } catch (err) {
      setError("Failed to load agent name");
    }
  };

  const fetchAgentPhone = async () => {
    try {
      const res = await fetch(`https://localhost:7025/api/DeliveryAgent/GetAgentPhone?id=${agentId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      const data = await res.text();
      setPhone(data);
      fetchAddresses(agentId); 
    } catch (err) {
      setError("Failed to load agent phone");
    }
  };

  const fetchAddresses = async (userId) => {
    try {
      const res = await fetch(`https://localhost:7025/api/Address/get/alladdress/users?userid=${userId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      const data = await res.json();
      console.log("Received addresses:", data);
      setAddresses(data);
      if (data.length > 0) {
        setNewAddress(data[0].address || data[0].address1);
        setAddressId(data[0].addressId);
      }
    } catch (error) {
      console.error("Error fetching addresses:", error);
      setError("Failed to load addresses");
    }
  };

  const handleUpdate = async () => {
    setError(null);
    setSuccess(null);
    try {
      const res = await fetch(
        `https://localhost:7025/api/Address/update/address?addressid=${addressId}&address=${encodeURIComponent(newAddress)}`,
        {
          method: "PUT",
          headers: { Authorization: `Bearer ${token}` }
        }
      );

      if (res.ok) {
        setSuccess("Address updated successfully!");
        setEditing(false);
        fetchAddresses(agentId); 
      } else {
        const errorText = await res.text();
        throw new Error(errorText || "Update failed");
      }
    } catch (err) {
      setError("Failed to update address: " + err.message);
    }
  };

  useEffect(() => {
    if (token && agentId) {
      fetchAgentName();
      fetchAgentPhone();
    }
  }, [token, agentId]);

  return (
    <div className="profile-container">
      <h2 className="profile-heading">Delivery Agent Profile</h2>
      {error && <p className="error-text">Error: {error}</p>}
      {success && <p className="success-text">{success}</p>}

      <div className="profile-details">
        <p><span className="label">Name:</span> {name}</p>
        <p><span className="label">Phone:</span> {phone}</p>

        <div className="address-section">
          <p><span className="label">Addresses:</span></p>
          {addresses.length === 0 ? (
            <p>No address found.</p>
          ) : (
            addresses.map((addr, index) => (
              <div key={addr.addressId} className="address-block">
                {editing && index === 0 ? (
                  <>
                    <input
                      type="text"
                      value={newAddress}
                      onChange={(e) => setNewAddress(e.target.value)}
                      className="input-field"
                    />
                    <button onClick={handleUpdate} className="save-button">Save</button>
                  </>
                ) : (
                  <>
                    <p><strong>Address {index + 1}:</strong> {addr.address || addr.address1}</p>
                 
                  </>
                )}
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default Profile;
