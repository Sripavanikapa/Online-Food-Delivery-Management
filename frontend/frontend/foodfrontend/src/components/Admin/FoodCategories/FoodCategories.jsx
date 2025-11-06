import React, { useState, useEffect } from "react";
import axios from "axios";
import './FoodCategories.css';
import { FaHamburger } from 'react-icons/fa';

const FoodCategories = () => {
  const [categories, setCategories] = useState([]);
  const [newCategoryName, setNewCategoryName] = useState("");
  const [newCategoryImage, setNewCategoryImage] = useState(null);
  const [editingId, setEditingId] = useState(null);
  const [editName, setEditName] = useState("");
  const [editImage, setEditImage] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem("token");
    const headers = { Authorization: `Bearer ${token}` };
    axios
      .get("https://localhost:7025/api/Admin/AllCategories", { headers })
      .then((res) => setCategories(res.data))
      .catch((err) => console.error("Error fetching categories:", err));
  }, []);

  const handleImageChange = (e) => {
    setNewCategoryImage(e.target.files[0]);
  };

  const handleEditImageChange = (e) => {
    setEditImage(e.target.files[0]);
  };

  const handleAddCategory = async () => {
    if (newCategoryName.trim() === "" || !newCategoryImage) {
      alert("Please provide both category name and image.");
      return;
    }
    const token = localStorage.getItem("token");
    const headers = { Authorization: `Bearer ${token}` };
    const formData = new FormData();
    formData.append("Name", newCategoryName);
    formData.append("Image", newCategoryImage);
    try {
      const response = await axios.post(
        "https://localhost:7025/api/Category/AddCategory",
        formData,
        { headers }
      );
      setCategories([...categories, response.data]);
      setNewCategoryName("");
      setNewCategoryImage(null);
      alert("Category added successfully!");
    } catch (error) {
      console.error("Error adding category:", error);
      alert("Failed to add category.");
    }
  };

  const handleCancelEdit = () => {
    setEditingId(null);
    setEditName("");
    setEditImage(null);
  };

  const handleEditCategory = async (id) => {
    if (editName.trim() === "") {
      alert("Please provide a category name.");
      return;
    }
    const token = localStorage.getItem("token");
    const headers = { Authorization: `Bearer ${token}` };
    const formData = new FormData();
    formData.append("Name", editName);
    if (editImage) {
      formData.append("Image", editImage);
    }
    try {
      const response = await axios.put(
        `https://localhost:7025/api/Category/UpdateCategory/${id}`,
        formData,
        { headers }
      );
      setCategories(
        categories.map((cat) =>
          cat.categoryId === id ? response.data : cat
        )
      );
      setEditingId(null);
      setEditName("");
      setEditImage(null);
      alert("Category updated successfully!");
    } catch (error) {
      console.error("Error updating category:", error);
      alert("Failed to update category. " + error.message);
    }
  };

  return (
    <div className="section-card-admin">
  <h2><FaHamburger style={{ marginRight: '10px' }} />Food Categories Management</h2>
  <div className="add-category-admin">
    <h3>Add New Category</h3>
    <input
      type="text"
      placeholder="Category Name"
      value={newCategoryName}
      onChange={(e) => setNewCategoryName(e.target.value)}
    />
    <input type="file" accept="image/*" onChange={handleImageChange} />
    {newCategoryImage && <span>{newCategoryImage.name}</span>}
    <button onClick={handleAddCategory}>Add Category</button>
  </div>
  <hr />
  <h3>Category List</h3>
  <div className="card-container-admin">
    {categories.map((category) => (
      <div key={category.categoryId} className="category-card-admin">
        {category.categoryId === editingId ? (
          <>
            <input
              type="text"
              value={editName}
              onChange={(e) => setEditName(e.target.value)}
            />
            <input
              type="file"
              accept="image/*"
              onChange={handleEditImageChange}
            />
            {editImage && <span>{editImage.name}</span>}
            <button onClick={() => handleEditCategory(category.categoryId)}>Save</button>
            <button onClick={handleCancelEdit}>Cancel</button>
          </>
        ) : (
          <>
            {category.imageUrl && (
              <img
                src={`https://localhost:7025/${category.imageUrl}`}
                alt={category.name}
                className="category-image-admin"
              />
            )}
            <h4>{category.name}</h4>
            <button onClick={() => {
              setEditingId(category.categoryId);
              setEditName(category.name);
              setEditImage(null);
            }}>
              Edit
            </button>
          </>
        )}
      </div>
    ))}
  </div>
</div>
  );
};

export default FoodCategories;