import React, { useEffect, useState } from "react";
import axios from "axios";
import './RestaurantCategory.css'
 
const RestaurantCategory = () => {
  const [categories, setCategories] = useState([]);
 
  useEffect(() => {
    axios.get("https://localhost:7025/api/Category/AllCategories")
      .then((response) => {
        setCategories(response.data);
      })
      .catch((error) => {
        console.error("Error fetching categories:", error);
      });
  }, []);
 
 
   
 
 
 
  return (
    <div className="category-main-div-res" >
      <h2 className="res-category-heading">List of Categories</h2>
    <div className="categories-grid-res">
      {categories.map((category, index) => (
        <div key={index} className="category-card-res">            
         
          <img
            src={`/${category.imageUrl}`}
            alt={category.name}
            style={{ width: "150px", height: "150px", objectFit: "cover" }}
          />
          <h4 id="category-id-res">{category.categoryId}</h4>
          <h3 id='category-name-res'>{category.name}</h3>
         
        </div>
       
      ))}
    </div>
    </div>
  );
};
 
export default RestaurantCategory;