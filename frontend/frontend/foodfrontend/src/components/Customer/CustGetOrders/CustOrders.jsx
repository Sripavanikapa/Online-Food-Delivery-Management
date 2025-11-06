import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './CustOrders.css';
import CustSearchBarNavbar from '../CustSearchBar/CustSearchBarNavbar';
import cart from '../../../assets/addtocart.png';
 
const CustOrders = () => {
  const [orders, setOrders] = useState([]);
  const userId = localStorage.getItem('userId');
 
  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const res = await fetch(`https://localhost:7025/api/Customer/Orders?userid=${userId}`);
        const data = await res.json();
        console.log(data);
        setOrders(data);
      } catch (error) {
        console.error('Error fetching orders:', error);
      }
    };
 
    if (userId) fetchOrders();
  }, [userId]);
 
 
 
   const navigate = useNavigate();
 
 
                  const [cartCount, setCartCount] = useState(0);
     
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
     
              useEffect(() => {
           const cart = JSON.parse(localStorage.getItem('cart')) || [];
           setCartCount(cart.length);
              }, []);
 
 
  return (
 
    <div class='custorders-main'>
      <div class='custorders-navbar'>
        <CustSearchBarNavbar></CustSearchBarNavbar>
      </div>
    <div className="orders-container-customer">
      <h2>Your Orders</h2>
       <button className="cart-button" onClick={goToCartPage}><img id='cart-image' src={cart} alt='cart'/>{cartCount}</button>
      {orders.length === 0 ? (
        <p>No orders found.</p>
       
      ) : (
        <div className="orders-list-customer">
          {orders.map((order, index) => (
            <div key={index} className="order-card-customer">
             <img src={order.foodItems[0]?.imageUrl} alt={order.foodItems[0]?.itemName} className="order-img-customer" />
              <div className="order-info-customer">
                <h3>{order.foodItems[0]?.itemName}</h3>
                <p><strong>Restaurant:</strong> {order.restaurantName}</p>
               
                <p><strong>Total Price:</strong> ₹{order.totalPrice}</p>
               <p>
  <strong>Status:</strong>{" "}
  <span className={order.status === "Pending" ? "food-status-pending" : "food-status-delivered"}>
    {order.status}
  </span>
</p>
              <p><strong>Date:</strong> {
  new Date(order.createdAt).toLocaleString("en-IN", {
    year: "numeric",
    month: "short",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
    hour12: true,
    timeZone: "Asia/Kolkata"
  })
}</p>

              </div>
            </div>
          ))}
        </div>
      )}
    </div>
    </div>
  );
};
 
export default CustOrders;
 