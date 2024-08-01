import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { GoogleLogin } from '@react-oauth/google';
import Header from "../Home/Header"
import axiosInstance from "../../Utils/axiosInstance"
import { toast } from "react-toastify"

const Login = () => {

    useEffect(() => {
        var token = localStorage.getItem("token");
        if (token != null) {
            navigate("/search")
        }
    })

    const [formData, setFormData] = useState({
        email: "",
        password: ""
    });

    const [errors, setErrors] = useState({});
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { id, value } = e.target;
        setFormData({
            ...formData,
            [id]: value
        });
    };

    const validate = () => {
        const newErrors = {};
        if (!formData.email) newErrors.email = "Email is required";
        if (!formData.password) newErrors.password = "Password is required";

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (validate()) {
            axiosInstance.post("/user/login", formData)
                .then(response => {
                    localStorage.setItem("token", response.data.token);
                    if (response.data.profilestatus === false) {
                        toast.warn("Please Complete your profile");
                        navigate("/addprofile");
                    }
                    navigate("/search");
                })
                .catch(error => {
                    toast.error(error.response.data.errorMessage);
                });
        }
    };

    const handleGoogleLoginSuccess = (credentialResponse) => {
        console.log("Google login successful", credentialResponse);
        const profile = credentialResponse.profileObj;
        axios.post('your-backend-api-url/google-login', profile)
            .then(res => {
                console.log(res.data);
                // handle success
                navigate("/dashboard");
            })
            .catch(err => {
                console.error(err);
                // handle error
            });
    };

    const handleGoogleLoginFailure = () => {
        console.error("Google login failed");
    };

    return (
        <>
            <Header />
            <div className="container d-flex align-items-center" style={{ height: "100vh" }}>
                <div className="row justify-content-center w-100">
                    <div className="col-lg-6">
                        <div className="card p-4 shadow">
                            <h3 className="mb-4 text-center text-primary fw-bolder">Login</h3>
                            <form onSubmit={handleSubmit}>
                                <div className="form-group mb-3">
                                    <label htmlFor="email">Email address</label>
                                    <input type="email" className="form-control" id="email" placeholder="Enter your email" value={formData.email} onChange={handleChange} />
                                    {errors.email && <small className="text-danger">{errors.email}</small>}
                                </div>
                                <div className="form-group mb-3">
                                    <label htmlFor="password">Password</label>
                                    <input type="password" className="form-control" id="password" placeholder="Enter your password" value={formData.password} onChange={handleChange} />
                                    {errors.password && <small className="text-danger">{errors.password}</small>}
                                </div>
                                <button type="submit" className="btn btn-primary btn-block w-100">Login</button>
                            </form>
                            <div className="text-center mt-4">
                                <p>Or Login With</p>
                                <GoogleLogin
                                    onSuccess={handleGoogleLoginSuccess}
                                    onError={handleGoogleLoginFailure}
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default Login;
