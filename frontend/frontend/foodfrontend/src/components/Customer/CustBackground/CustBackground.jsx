import React from 'react';
import './CustBackground.css'; 
import CustNavbar from '../CustNavbar/CustNavbar';
import chopsticks from '../../../assets/chopsticks.png'
import onion from '../../../assets/onion.png'
import burger from '../../../assets/noto_hamburger.png'
import  fork from '../../../assets/fork.png';
import broccoli from '../../../assets/broccoli.png'
const CustBackground = () => {
    return (
        <div>
        <div className="background-container">
      
            <div className="background-left-section">
             
                
            </div>

     
            <div className="background-right-section">
                <div className="hero-text-content">
                    
                    <h1 className='background-text'>
                         
                        Order food.

                        <br />
                        Discover best restaurants.
                        <br />
                        Foodeli it!
                    </h1>
                    <img src={chopsticks} alt="chopsticks" className="chopsticks" />
                         <img src={onion} alt="onion" className="onion" />
                         <img src={burger} alt="burger" className="burger" />
                         <img src={fork} alt="fork" className="fork" />
                          <img src={broccoli} alt="broccoli" className="broccoli" />
                </div>
            </div>
             
        </div>
        
       </div>
    );
};

export default CustBackground;