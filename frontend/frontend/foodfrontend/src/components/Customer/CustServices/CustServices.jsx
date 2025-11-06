import React from 'react'
import order from '../../../assets/Order_food-pana.png'
import delivery from '../../../assets/Take Away-rafiki.png'
import waiter from '../../../assets/waiters.png'
import './CustServices.css'

 const CustServices = () => {
  return (
    <div className='main-component'>
           <p className='service-subheading' id='service-subheading'>WHAT WE SERVE</p>
           <h1 className='service-heading' id='service-heading' ><b>Your Favourite Food<br/> Delivery Partner</b></h1>
        <div className="container text-center" id='services'>
                <div className="row">
                    <div className="col" id='first-col-service'>
                           <img src={order} alt="service1"></img>
                           <h3>Easy to Order</h3>
                           <p>You only need a few steps in ordering food</p>
                    </div>
                    <div className="col" id='second-col-service'>
                           <img src={delivery} alt="service2"></img>
                           <h3>Fastest Delivery</h3>
                           <p>Delivery that is always ontime even faster</p>
                    </div>
                    <div className="col" id='third-col-service'>
                          <img src={waiter} alt="service3"></img>
                          <h3>Best Quality</h3>
                          <p>Not only fast for us quality is also number one</p>
                    </div>
                    
                </div>
                </div>
    </div>
  )
}

export default CustServices;
