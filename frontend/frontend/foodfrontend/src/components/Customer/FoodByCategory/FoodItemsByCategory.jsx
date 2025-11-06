import React, { useEffect, useState } from "react";
import axios from "axios";
import { useLocation, useNavigate } from "react-router-dom"; 
import './FoodItemsByCategory.css';

const FoodItemsByCategory = () => {
  const [foodItems, setFoodItems] = useState([]);
  const location = useLocation();
  const navigate = useNavigate(); 
  const queryParams = new URLSearchParams(location.search);
  const categoryName = queryParams.get("query");

  useEffect(() => {
    if (categoryName) {
      axios.get(`https://localhost:7025/api/FoodItem/ByCategory?category=${categoryName}`)
        .then((res) => {
          setFoodItems(res.data);
        })
        .catch((err) => {
          console.error("Error fetching food items:", err);
        });
    }
  }, [categoryName]);

  const handleAddToCart = (item) => {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];

    const cartItem = {
      itemId: item.itemId,
      itemName: item.itemName,
      foodName: item.foodName || item.itemName,
      imageUrl: item.imageurl,
      description: item.description,
      price: item.price
    };

    const updatedCart = [...cart, cartItem];
    localStorage.setItem('cart', JSON.stringify(updatedCart));
    alert(`${item.itemName} added to cart`);
  };

  const handleGoToCart = () => {
    navigate('/cart');
  };

  return (
    <div className="fooditems-category-container">
      <h2 className="fooditems-category-title">{categoryName} Food Items</h2>

     
      <div className="go-to-cart-wrapper">
        <button className="go-to-cart-btn" onClick={handleGoToCart}>
          Go to Cart 
        </button>
      </div>

      {foodItems.length === 0 ? (
        <p className="fooditems-category-empty">No food items found.</p>
      ) : (
        <div className="fooditems-grid">
          {foodItems.map((item, index) => (
            <div key={index} className="fooditem-card">
              <img src={item.imageurl} alt={item.itemName} className="fooditem-image" />
              <div className="fooditem-details">
                <h3 className="fooditem-name">{item.itemName}</h3>
                <p className="fooditem-price">₹{item.price}</p>
                <p className="fooditem-description">{item.description}</p>
                <button className="add-to-cart-btn" onClick={() => handleAddToCart(item)}>
                  Add to Cart
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default FoodItemsByCategory;
