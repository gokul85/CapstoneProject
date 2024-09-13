import React from 'react';
import RegistrationForm from "../Home/RegistrationForm"
import { useNavigate } from "react-router-dom"
import Header from "../Home/Header"
import { useEffect } from 'react';

const Register = () => {
    const navigate = useNavigate()
    useEffect(() => {
        var token = localStorage.getItem("token");
        if (token !== "" || !token) {
            navigate("/search")
        }
    })

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
