import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import cartIcon from '../../../assets/addtocart.png';
import CustNavbar from '../CustNavbar/CustNavbar';
import './OrderNow.css';

const OrderNow = () => {
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const keyword = searchParams.get('keyword');

  const [foodItem, setFoodItem] = useState(null);
  const [addresses, setAddresses] = useState([]);
  const [selectedAddressId, setSelectedAddressId] = useState('');
  const [orderPlaced, setOrderPlaced] = useState(false);
  const [showAddInput, setShowAddInput] = useState(false);
  const [newAddress, setNewAddress] = useState('');
  const [quantity, setQuantity] = useState(1);
  const [cartCount, setCartCount] = useState(0);

  const userId = localStorage.getItem('userId');
  const token = localStorage.getItem('token');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchFoodItem = async () => {
      try {
        const res = await fetch(`https://localhost:7025/api/FoodItem/search?keyword=${keyword}`, {
          headers: { Authorization: `Bearer ${token}` }
        });
        const data = await res.json();
        console.log("Fetched food items:", data);
      if (data.length > 0) {
  const enrichedItem = {
    ...data[0],
    itemId: data[0].itemId,
    restaurant_id: data[0].restaurant_id
  };
  setFoodItem(enrichedItem);
}else {
          alert("No food items found for keyword: " + keyword);
        }
      } catch (error) {
        console.error('Error fetching food item:', error);
      }
    };

    const fetchAddresses = async () => {
      try {
        const res = await fetch(`https://localhost:7025/api/Address/get/alladdress/users?userid=${userId}`, {
          headers: { Authorization: `Bearer ${token}` }
        });
        const data = await res.json();
        console.log("Received addresses:", data);
        setAddresses(data);
      } catch (error) {
        console.error('Error fetching addresses:', error);
      }
    };

    if (keyword) fetchFoodItem();
    if (userId) fetchAddresses();
  }, [keyword, userId, token]);

  useEffect(() => {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    setCartCount(cart.length);
  }, []);

  useEffect(() => {
    if (orderPlaced) {
      alert("Order Placed Successfully");
    }
  }, [orderPlaced]);

  const handleOrderSubmit = async () => {
    if (!foodItem || !foodItem.itemId || !selectedAddressId) {
      alert("Missing required fields.");
      return;
    }

    const payload = {
      userId: parseInt(userId),
      restaurantId: parseInt(foodItem.restaurant_id),
      custAddressId: parseInt(selectedAddressId),
      status: "Pending",
      createdAt: new Date().toISOString(),
      items: [
        {
          itemId: parseInt(foodItem.itemId),
          quantity: quantity
        }
      ]
    };

    console.log("Order payload:", payload);

    try {
      const res = await fetch('https://localhost:7025/api/Customer/OrderNow', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(payload)
      });

      if (!res.ok) throw new Error('Failed to place order');

      const cart = JSON.parse(localStorage.getItem('cart')) || [];
      const updatedCart = cart.filter(item => item.itemId !== foodItem.itemId);
      localStorage.setItem('cart', JSON.stringify(updatedCart));

      setOrderPlaced(true);
      alert("Order Placed Successfully");
    } catch (error) {
      console.error('Error placing order:', error);
      
    }
  };

  const handleAddAddress = async () => {
    if (!newAddress.trim()) {
      alert("Please enter a valid address.");
      return;
    }

    try {
      const payload = {
        cust_id: parseInt(userId),
        address1: newAddress
      };

      const res = await fetch('https://localhost:7025/api/Address/create/address', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(payload)
      });

      if (!res.ok) throw new Error("Server responded with an error.");

      alert("Address added successfully.");
      setNewAddress('');
      setShowAddInput(false);

      const updatedRes = await fetch(`https://localhost:7025/api/Address/get/alladdress/users?userid=${userId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      const updatedData = await updatedRes.json();
      setAddresses(updatedData);
    } catch (error) {
      console.error('Error adding address:', error);
      alert(`Failed to add address. ${error.message}`);
    }
  };

  const handleDeleteAddress = async (addressId) => {
    const confirmDelete = window.confirm("Are you sure you want to delete this address?");
    if (!confirmDelete) return;

    try {
      const res = await fetch(`https://localhost:7025/api/Address/delete/address?id=${addressId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`
        }
      });

      const resultText = await res.text();
      const result = resultText.trim().toLowerCase() === 'true';

      if (result) {
        setAddresses(prev => prev.filter(addr => addr.addressId !== addressId));
        alert("Address deleted successfully.");
      } else {
        throw new Error("Address could not be deleted.");
      }
    } catch (error) {
      console.error('Error deleting address:', error);
      alert(`Failed to delete address. ${error.message}`);
    }
  };

  const handleAddToCart = (item) => {
    const existingCart = JSON.parse(localStorage.getItem('cart')) || [];
    const updatedCart = [...existingCart, item];
    localStorage.setItem('cart', JSON.stringify(updatedCart));
    setCartCount(updatedCart.length);
    alert(`${item.itemName} added to cart!`);
  };

  const goToCartPage = () => {
    navigate('/cart');
  };

  return (
    <div className="order-page">
      <div className='navbar-order'>
        <CustNavbar />
      </div>

      <div className='order-header'>
        <h2>Order Summary</h2>
        <button className="cart-button" onClick={goToCartPage}>
          <img id='cart-image' src={cartIcon} alt='cart' />{cartCount}
        </button>
      </div>

      {foodItem && (
        <div className="order-summary">
          <img src={`https://localhost:7025${foodItem.imageurl}`} alt={foodItem.itemName} className="order-image" />
          <div className="order-details">
            <h3>{foodItem.itemName}</h3>
            <p>{foodItem.description}</p>
            <p><strong>Price:</strong> ₹{foodItem.price}</p>
          </div>
          <div className="quantity-section">
            <label>Quantity:</label>
            <button onClick={() => setQuantity(prev => Math.max(1, prev - 1))} className="quantity-btn">-</button>
            <span className="quantity-value">{quantity}</span>
            <button onClick={() => setQuantity(prev => prev + 1)} className="quantity-btn">+</button>
          </div>
        </div>
      )}

      <div className='address-order'>
        <h3>Select Delivery Address:</h3>

        <div className="add-address-section">
          {!showAddInput ? (
            <div className="add-address-container">
              <button onClick={() => setShowAddInput(true)} className="add-address-btn">+ Add Address</button>
            </div>
          ) : (
            <div className="add-address-form">
              <input
                type="text"
                value={newAddress}
                onChange={(e) => setNewAddress(e.target.value)}
                placeholder="Enter new address"
                className="add-address-input"
              />
              <button onClick={handleAddAddress} className="add-address-btn">Save</button>
              <button onClick={() => {
                setShowAddInput(false);
                setNewAddress('');
              }} className="cancel-address-btn">Cancel</button>
            </div>
          )}
        </div>

        <div className="address-list">
          {addresses.length === 0 ? (
            <p>No addresses found. Please add one.</p>
          ) : (
            addresses.map(addr => (
              <div key={addr.addressId} className="address-card">
                <div className="address-left">
                  <input
                    type="checkbox"
                    checked={selectedAddressId === addr.addressId?.toString()}
                    onChange={() => setSelectedAddressId(addr.addressId?.toString())}
                  />
                  <span>{addr.address}</span>
                </div>
                <button className="delete-btn" onClick={() => handleDeleteAddress(addr.addressId)}>Delete</button>
              </div>
            ))
          )}
        </div>

        <br />
        <button
          onClick={handleOrderSubmit}
          disabled={!selectedAddressId}
          className="confirm-order-button"
        >
          Confirm Order
        </button>
      </div>
    </div>
  );
};

export default OrderNow;
