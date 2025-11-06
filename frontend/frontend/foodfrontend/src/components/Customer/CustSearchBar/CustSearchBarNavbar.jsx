import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../CustNavbar/CustNavbar.css'
import icon from '../../../assets/foodeli-icon.png';
import profile from '../../../assets/profile.png';
import { Link } from 'react-router-dom';
const CustSearchBarNavbar = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    const handleKeyPress = (e) => {
        if (e.key === 'Enter' && searchTerm.trim()) {
            navigate(`/fooditems/results?query=${encodeURIComponent(searchTerm.trim())}`);
        }
    };

    return (
        <nav className="full-navbar-container">
            <div className="navbar-logo-area">
                <div className="logo-box">
                    <img src={icon} alt="Foodeli Logo" className="logo-icon" />
                </div>
                 <div className="logo-link">
             <a href="/" style={{textDecoration:'none'}}><span className="logo-text">Foodeli</span></a>
  

           </div>
            </div>

            <div className="navbar-content-area">
                <div className="nav-links-group">
                   <Link to="/customer">Home</Link>
                   <Link to="/customer#category-section">Category</Link> 
                   <Link to="/orders">Orders</Link>
                    <Link to="/customer#brands-section">Brands</Link>
              </div>

              

                <div className="search-bar-box-customer">
                    <i className="fas fa-search search-icon-customer"></i>
                    <input
                        type="text"
                        placeholder="Search Food Items"
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        onKeyPress={handleKeyPress}
                    />
                </div>

                 <button className="profile-btn">
                 <a href="/profile"><img src={profile}  alt="Profile Logo"  /></a>
               </button>
            </div>
        </nav>
    );
};

export default CustSearchBarNavbar;