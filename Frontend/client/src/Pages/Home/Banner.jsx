import React from 'react';
import RegistrationForm from './RegistrationForm';
import "./Banner.css";

const Banner = () => {
    return (
        <div className="container mt-5 bannertextarea">
            <div className="row align-items-center">
                <div className="col-lg-6 text-center text-lg-left mb-4 mb-lg-0">
                    <h1 className="font-weight-bold">Every Love Story is Beautiful <span className='text-primary'>Make Yours Special</span></h1>
                </div>
                <div className="col-lg-6">
                    <RegistrationForm />
                </div>
            </div>
        </div>
    );
};

export default Banner;
