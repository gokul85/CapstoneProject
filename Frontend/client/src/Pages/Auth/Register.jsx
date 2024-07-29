import React from 'react';
import RegistrationForm from "../Home/RegistrationForm"
import Header from "../Home/Header"

const Register = () => {

    return (
        <>
            <Header />
            <div className="container d-flex align-items-center pt-5 mt-5" style={{ height: "120vh" }}>
                <div className="row justify-content-center w-100">
                    <div className="col-lg-6">
                        <RegistrationForm />
                    </div>
                </div>
            </div>
        </>
    );
};

export default Register;
