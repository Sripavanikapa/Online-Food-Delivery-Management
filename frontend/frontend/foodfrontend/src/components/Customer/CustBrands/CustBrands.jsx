import React from 'react';
import './CustBrands.css';
import macdonalds from '../../../assets/macdonals.png'
import burgerking from '../../../assets/burgerking.png'
import andhrakaaram from '../../../assets/andhara kaaram.png'
import kfc from '../../../assets/kfc.png'
import a2b from '../../../assets/a2b.png'
const brands = [
   
    { name: "McDonald\'s", logo: macdonalds },
    { name: "Burger King", logo: burgerking },
    { name: "Andhra Karam", logo: andhrakaaram },
    { name: "A2B", logo: a2b },
    { name: "KFC", logo: kfc }
];

const CustBrands = () => {
    return (
        <div className="brands-container">
            <h2 className='brand-header'><b>Top brands for you</b></h2>
            <div className="brands-list">
                {brands.map((brand, index) => (
                    <div key={index} className="brand-item">
                        <img src={brand.logo} alt={`${brand.name} logo`} />
                       <b><p className="brand-name">{brand.name}</p></b> 
                        
                    </div>
                ))}
            </div>
        </div>
    );
};

export default CustBrands;