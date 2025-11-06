import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from 'react-router-dom';
import './CustCategory.css'

const CustCategory = () => {
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    axios.get("https://localhost:7025/api/Category/AllCategories") 
      .then((response) => {
        console.log(response);
        setCategories(response.data);
      })
      .catch((error) => {
        console.error("Error fetching categories:", error);
      });
  }, []);

  
     const navigate = useNavigate();
      const handleCategoryClick = (categoryName) => {
  navigate(`/fooditems/category?query=${categoryName}`);
};



  return (
    <div className="category-main-div">
      <div className="header-div">
        <h1 className="category-header" id="category-header"><b>Order our best food options</b></h1>
        </div>
    <div className="categories-grid">
      {categories.map((category, index) => (

        <div key={index} className="category-card">            
           <button
            onClick={() => handleCategoryClick(category.name)}
            style={{ border: "none", background: "none", cursor: "pointer" }}>

          <img
            src={`/${category.imageUrl}`} 
            alt={category.name}
            style={{ width: "150px", height: "150px", objectFit: "cover" }}
          />
           
          <h3 id='category-name'>{category.name}</h3>
          </button>
        </div>
        
      ))}
    </div>
    </div>
  );
};

export default CustCategory;