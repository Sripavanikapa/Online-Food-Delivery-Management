import React from 'react';
import { useNavigate } from 'react-router-dom';
import CustNavbar from '../../Customer/CustNavbar/CustNavbar'
import './Cart.css';

const Cart = () => {
  const navigate = useNavigate();
  const cartItems = JSON.parse(localStorage.getItem('cart')) || [];


  const handleRemoveItem = (itemId) => {
    const updatedCart = cartItems.filter(item => item.itemId !== itemId);
    localStorage.setItem('cart', JSON.stringify(updatedCart));
    window.location.reload();
  };

  const handleOrderNow = (item) => {
    navigate(`/ordernow?keyword=${encodeURIComponent(item.itemName)}`);

  };

  return (
    <div className="cart-page">   
        <div className='navbar-cart'>
        <CustNavbar/>
        
      </div>
      <div className='cart-items'>
      {cartItems.length === 0 ? (
        <p>No items in cart.</p>
      ) : (
        cartItems.map(item => (
          <div key={item.itemId} className="cart-item">
            <img src={item.imageUrl} alt={item.foodName} className="cart-image" />
            <div className="cart-details">
              <h4>{item.foodName}{item.itemName}</h4>
              <p>{item.description}</p>
              <p><strong>Price:</strong> ₹{item.price}</p>
              <div className="cart-buttons">
                <button onClick={() => handleRemoveItem(item.itemId)}>Remove</button>
                <button onClick={() => handleOrderNow(item)}>Order Now</button>
              </div>
            </div>
          </div>
          
        ))
      )}
    </div>
    </div>
  );
};

export default Cart;
