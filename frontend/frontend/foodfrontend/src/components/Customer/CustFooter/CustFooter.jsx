import './CustFooter.css'
import icon from '../../../assets/foodeli-icon.png'
import React from 'react'
import facebook from '../../../assets/facebook.png'
import instagram from '../../../assets/instagram.png'
import email from '../../../assets/email.png'

 const CustFooter = () => {
  return (
        <div class="container text-center" id='footer'>
                <div class="row" >
                            <div class="col" id='first-col1'>
                               <h4 id='footer-heading'><img src={icon} alt='icon' id='footer-icon'></img>Foodeli</h4>
                              
                               <p id='footer-p'>Our job is to filling your tummy with delicious food and with fast and free delivery. We believe great food should be accessible to everyone, delivered quickly and without extra cost. Your next favorite meal is just a few clicks away!</p>
                            </div>
                            <div class="col" id='sec-col'>
                                 <h4>Our Location</h4>
                                 <p id='footer-p'>AG Womens Pg, Rajiv Gandhi Arcade, Egattur, Opposite Marina Mall, Chennai - 530603</p>
                            </div>
                            <div class="col" id='third-col'>
                               
                                <h4>Contact Us</h4>
                                <img src={facebook} alt='facebook' id='contact'></img>
                                  <a href="https://www.instagram.com/foodeliapp/" ><img src={instagram} alt='instagram' id='contact'></img></a>
                                <br></br>
                                 <button className="footer-btn">
                                 <a href="https://mail.google.com/" ><img src={email}  alt="email" id='contact'  /></a>
                                 </button>
                            </div>
                </div>
        </div>
  )
}

export default CustFooter;
