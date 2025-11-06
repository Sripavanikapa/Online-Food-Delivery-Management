import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from 'axios';
import '../CustSearchBar/CustSearchBar.css';
import cart from '../../../assets/addtocart.png';
import CustSearchBarNavbar from './CustSearchBarNavbar';

const CustSearchBar = () => {
  const [foodItems, setFoodItems] = useState([]);
  const [cartCount, setCartCount] = useState(0);
  const location = useLocation();
  const navigate = useNavigate();

  const searchParams = new URLSearchParams(location.search);
  const query = searchParams.get('query');

  useEffect(() => {
    const fetchData = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        console.warn('No token found. User may not be logged in.');
        return;
      }

      const headers = {
        Authorization: `Bearer ${token}`
      };

      try {
        const keywordResponse = await axios.get(
          `https://localhost:7025/api/Customer/SearchFoodItems?keyword=${query}`,
          { headers }
        );

        if (keywordResponse.data.length > 0) {
          setFoodItems(keywordResponse.data);
        } else {
          const restaurantResponse = await axios.get(
            `https://localhost:7025/api/Customer/get/foodItemsByRestaurant/${query}`,
            { headers }
          );
          setFoodItems(restaurantResponse.data);
        }
      } catch (error) {
        console.error('Error fetching food items:', error);
      }
    };

    if (query) fetchData();
  }, [query]);

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
    <div className='complete-page'>
      <div className='navbar-fooditems'>
        <CustSearchBarNavbar />
      </div>

      <button className="cart-button" onClick={goToCartPage}>
        <img id='cart-image' src={cart} alt='cart' />{cartCount}
      </button>

      <div className="food-items-container">
        {foodItems.length > 0 &&
          foodItems.map(item => (
            <div key={item.item_id} className="food-card">
              <img src={item.imageUrl} alt={item.name} id='image-food' />
              <div className='left-div-food'>
                <h6>{item.itemName}</h6>
                <p><strong>Price:</strong> ₹{item.price}</p>
              </div>
              <div className='right-div-food' style={{ marginBottom: '10%' }}>
                <p>{item.description}</p>
                <button className="order-button" onClick={() => handleAddToCart(item)}>Add To Cart</button>
              </div>
            </div>
          ))
        }
      </div>
    </div>
  );
};

export default CustSearchBar;
