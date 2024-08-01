// Checkout.js
import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import axiosInstance from "../../Utils/axiosInstance"

const Checkout = () => {
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);

    const handleSubscribe = async () => {
        setLoading(true);
        var res = await axiosInstance.post("/premium/subscribe", { Amount: 999 });
        console.log(res.data);
        if (res.data.result) {
            var response = await axiosInstance.get("/user/refreshtoken");
            console.log(response.data);
            if (response.data) {
                localStorage.setItem("token", response.data);
            }
        }
        setLoading(false);
        toast.success("Subscribed Successfully!");
        navigate('/search');
    };

    return (
        <div className="d-flex justify-content-center align-items-center min-vh-100 bg-light">
            <div className="card shadow p-3 mb-5 bg-white rounded" style={{ maxWidth: '400px', width: '100%' }}>
                <div className="card-body text-center">
                    <h3 className="card-title mb-4">Checkout</h3>
                    <div className="mb-4">
                        <p>You're about to subscribe to the Premium Account. Enjoy all the premium features!</p>
                        <h4>₹999 / Month</h4>
                    </div>
                    <button className='btn btn-success btn-block' onClick={handleSubscribe} disabled={loading}>Subscribe</button>
                </div>
            </div>
        </div>
    );
};

export default Checkout;
