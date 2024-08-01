// Premium.js
import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useNavigate } from 'react-router-dom';

const Premium = () => {
    const navigate = useNavigate();

    const handleCheckout = () => {
        navigate('/checkout');
    };

    return (
        <div className="d-flex justify-content-center align-items-center min-vh-100 bg-light">
            <div className="card shadow p-3 mb-5 bg-white rounded" style={{ maxWidth: '400px', width: '100%' }}>
                <div className="card-body text-center">
                    <h3 className="card-title mb-4">Premium Account</h3>
                    <ul className="list-unstyled mb-4">
                        <li className="mb-2"><i className="bi bi-check-circle-fill text-success"></i> Premium Users can see the contact details</li>
                        <li className="mb-2"><i className="bi bi-check-circle-fill text-success"></i> Premium Users can only see 5 contacts per day</li>
                    </ul>
                    <h4 className="mb-4">₹999 / Month</h4>
                    <button className="btn btn-primary btn-block" onClick={handleCheckout}>Go to Checkout</button>
                </div>
            </div>
        </div>
    );
};

export default Premium;
