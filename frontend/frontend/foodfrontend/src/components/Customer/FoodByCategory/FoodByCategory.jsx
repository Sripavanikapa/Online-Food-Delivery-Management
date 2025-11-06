import React, { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './FoodByCategory.css';
import cart from '../../../assets/addtocart.png';
import CustSearchBarNavbar from '../CustSearchBar/CustSearchBarNavbar';


const FoodByCategory = () => {
  const [activeSection, setActiveSection] = useState('home');
  const [foodItems, setFoodItems] = useState([]);
  const location = useLocation();

  const searchParams = new URLSearchParams(location.search);
  const query = searchParams.get('query');

        useEffect(() => {
        
        const fetchData = async () => {
        try {
            const categoryResponse = await axios.get(`https://localhost:7025/api/FoodItem/GetFoodItemsbycategory?CategoryName=${query}`);
            setFoodItems(categoryResponse.data);
        } catch (error) {
            console.error('Error fetching food items by category:', error);
        }
        };


    if (query) fetchData();
  }, [query]);

  const navigate = useNavigate();

   
                const [cartCount, setCartCount] = useState(0);

                  const handleAddToCart = (item) => {
                  const existingCart = JSON.parse(localStorage.getItem('cart')) || [];
                  const updatedCart = [...existingCart, item];
                  localStorage.setItem('cart', JSON.stringify(updatedCart));
                  setCartCount(updatedCart.length); 
                  alert(`${item.foodName} added to cart!`);
                };


                  const goToCartPage = () => {
                    navigate('/cart');
                  };

                 useEffect(() => {
                    const cart = JSON.parse(localStorage.getItem('cart')) || [];
                    setCartCount(cart.length);
                  }, []);
  return (
    <>
    
    <div className='complete-page'>
      <div className='navbar-fooditems'>
        <CustSearchBarNavbar setActiveSection={setActiveSection}  />
        
      </div>
        <button className="cart-button" onClick={goToCartPage}><img id='cart-image' src={cart} alt='cart'/>{cartCount}</button>
      <div className="food-items-container" >
        {foodItems.length > 0 &&
          foodItems.map(item => (
             <div key={item.item_id} className="food-card">
              <img src={item.imageurl} alt={item.name} id='image-food'/>
              <div className='left-div-food'>
              <h6>{item.foodName}</h6>
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
    </>
  );
};

export default FoodByCategory;