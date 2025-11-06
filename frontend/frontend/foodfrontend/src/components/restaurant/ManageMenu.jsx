import React, { useEffect, useState } from 'react';
import './ManageMenu.css';
import axios from 'axios';
import menuIcon from '../../assets/menu icon.png';
import foodItem from '../../assets/food item.png';

const ManageMenu = () => {
  const [menuItems, setMenuItems] = useState([]);
  const [imagePreview, setImagePreview] = useState(null);
  const [editingItem, setEditingItem] = useState(null);
  const [formData, setFormData] = useState({
    item_name: '',
    price: '',
    description: '',
    category_id: '',
    status: true,
    keywords: '',
    image: null
  });

  const restaurantId = localStorage.getItem('userId');
  const token = localStorage.getItem('token');

  useEffect(() => {
    if (!restaurantId || !token) return;

    axios.get('https://localhost:7025/api/Restaurant/RestaurantWithFoodItems', {
      headers: { Authorization: `Bearer ${token}` }
    }).then(res => {
      const myRestaurant = res.data.find(r => r.restaurantId === parseInt(restaurantId));
      console.log("Fetched menu items:", myRestaurant?.foodItems);
      setMenuItems(myRestaurant?.foodItems || []);
    }).catch(err => {
      console.error('Failed to fetch menu items:', err);
    });
  }, [restaurantId, token]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    setFormData(prev => ({ ...prev, image: file }));
    setImagePreview(URL.createObjectURL(file));
  };

const handleSubmit = async () => {
  const data = new FormData();
  data.append('restaurant_id', restaurantId);
  data.append('itemName', formData.item_name);
  data.append('price', formData.price);
  data.append('category_id', formData.category_id);
  data.append('status', formData.status);
  data.append('Description', formData.description);
  data.append('keywords', formData.keywords);
  data.append('image', formData.image);
  data.append('Quantity', 0); 
  data.append('imageurl', 'placeholder.png'); 
  data.append('CategoryName', 'Unknown');   

  try {
    await axios.post('https://localhost:7025/api/Restaurant/upload-fooditem', data, {
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'multipart/form-data'
      }
    });
    alert('Food item added!');
    window.location.reload();
  } catch (err) {
    console.error('Upload failed:', err.response?.data?.errors || err.message);
    alert('Upload failed. Check console for details.');
  }
};




  const handleEdit = (item) => {
    setEditingItem(item);
    setFormData({
      item_name: item.itemName,
      price: item.price,
      description: item.description,
      category_id: item.category_Id || item.category_id,
      status: item.status,
      keywords: item.keywords || '',
      image: null
    });
    console.log(formData);
    setImagePreview(item.imageUrl ? `https://localhost:7025${item.imageUrl}` : null);
  };

  const cancelEdit = () => {
    setEditingItem(null);
    setFormData({
      item_name: '',
      price: '',
      description: '',
      category_id: '',
      status: true,
      keywords: '',
      image: null
    });
    setImagePreview(null);
  };

  const handleUpdate = async () => {
  const data = new FormData();
  data.append('foodid', editingItem.itemId);
  data.append('restaurant_id', restaurantId);
  data.append('itemName', formData.item_name);
  data.append('price', formData.price);
  data.append('category_id', formData.category_id);
  data.append('status', formData.status);
  data.append('Description', formData.description);
  data.append('keywords', formData.keywords);
  data.append('Quantity', 0); 

 
  data.append('imageurl', editingItem.imageurl || 'placeholder.png');
  data.append('CategoryName', editingItem.CategoryName || 'Unknown');

  if (formData.image) {
    data.append('image', formData.image);
  }

  try {
    await axios.put('https://localhost:7025/api/Restaurant/update/fooditem', data, {
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'multipart/form-data'
      }
    });
    alert('Food item updated!');
    window.location.reload();
  } catch (err) {
    console.error('Update failed:', err.response?.data?.errors || err.message);
    alert('Update failed. Check console for details.');
  }
};

  const handleDelete = async (itemId) => {
    try {
      await axios.delete(`https://localhost:7025/api/Restaurant/delete/fooditem/${itemId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      alert('Item deleted');
      setMenuItems(prev => prev.filter(item => item.itemId !== itemId));
    } catch (err) {
      alert('Delete failed');
      console.error(err);
    }
  };

  return (
    <div className="menu-manager">
      <h2><img src={foodItem} alt="food item" /> {editingItem ? 'Edit Food Item' : 'Add New Food Item'}</h2>

      <div className="menu-form">
        <input name="item_name" placeholder="Item Name" value={formData.item_name} onChange={handleChange} />
        <input name="price" type="number" placeholder="Price" value={formData.price} onChange={handleChange} />
        <textarea name="description" placeholder="Description" value={formData.description} onChange={handleChange} />
        <input name="category_id" type="number" placeholder="Category ID" value={formData.category_id} onChange={handleChange} />
        <input name="keywords" placeholder="Keywords" value={formData.keywords} onChange={handleChange} />
        <div className="checkbox-group">
          <label htmlFor="status">Available:</label>
          <input
            id="status"
            name="status"
            type="checkbox"
            checked={formData.status}
            onChange={handleChange}
          />
        </div>

        <input type="file" accept="image/*" onChange={handleImageChange} />
        {imagePreview && <img src={imagePreview} alt="Preview" className="preview-img" />}
        <button className="submit-button" onClick={editingItem ? handleUpdate : handleSubmit}>
          {editingItem ? 'Update Item' : 'Add Item'}
        </button>
        {editingItem && <button onClick={cancelEdit}>Cancel</button>}
      </div>

      <div className="menu-list">
        <h3><img src={menuIcon} alt="menu" /> Current Menu</h3>
        <div className="menu-grid">
          {Array.isArray(menuItems) && menuItems.map(item => (
            <div key={item.itemId} className="menu-card">
              <img
                src={item.imageUrl ? `https://localhost:7025${item.imageUrl}` : '/default-food.png'}
                alt={item.itemName}
                className="menu-img"
                onError={(e) => {
                  e.target.onerror = null;
                  e.target.src = '/default-food.png';
                }}
              />
              <h4>{item.itemName}</h4>
              <p>{item.description}</p>
              <p>₹{item.price}</p>
              <button onClick={() => handleEdit(item)}>Edit</button>
              <button onClick={() => handleDelete(item.itemId)}>Delete</button>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ManageMenu;
