import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosInstance from "../../Utils/axiosInstance"
import { toast } from "react-toastify"

const RegistrationForm = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        onBehalf: "Self",
        firstName: "",
        lastName: "",
        gender: "Male",
        dob: "",
        email: "",
        phone: "",
        password: "",
        confirmPassword: ""
    });
    const [errors, setErrors] = useState({});

    const handleChange = (e) => {
        const { id, value } = e.target;
        setFormData((prevFormData) => {
            let newGender = prevFormData.gender;
            if (id === "onBehalf") {
                newGender = getGender(value);
            }
            return {
                ...prevFormData,
                [id]: value,
                ...(id === "onBehalf" && { gender: newGender })
            };
        });
    };

    const getGender = (onBehalf) => {
        switch (onBehalf) {
            case "Self":
            case "Brother":
            case "Son":
            case "Friend":
                return "Male";
            case "Sister":
            case "Daughter":
                return "Female";
            default:
                return "Male";
        }
    };


    const validate = () => {
        const newErrors = {};
        if (!formData.firstName) newErrors.firstName = "First name is required";
        if (!formData.lastName) newErrors.lastName = "Last name is required";
        if (!formData.dob) newErrors.dob = "Date of Birth is required";
        if (!formData.email) newErrors.email = "Email is required";
        if (!formData.phone) newErrors.phone = "Phone number is required";
        if (!formData.password) newErrors.password = "Password is required";
        if (formData.password !== formData.confirmPassword) newErrors.confirmPassword = "Passwords do not match";

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (validate()) {
            try {
                var response = await axiosInstance.post("/user/register", formData);
                toast.success("User Registration Successful")
                toast.success("Please login to create Profile")
                navigate("/login");
            } catch (error) {
                toast.error(error.response.data.errorMessage);
            }
        }
    };

    return (
        <div className="card p-4 shadow">
            <h3 className="mb-4 text-center text-primary fw-bolder">Create Your Account</h3>
            <form onSubmit={handleSubmit}>
                <div className="form-group mb-3">
                    <label htmlFor="onBehalf">Registering On Behalf Of</label>
                    <select
                        className="form-control"
                        id="onBehalf"
                        value={formData.onBehalf}
                        onChange={handleChange}
                    >
                        <option value="Self">Self</option>
                        <option value="Brother">Brother</option>
                        <option value="Sister">Sister</option>
                        <option value="Friend">Friend</option>
                        <option value="Daughter">Daughter</option>
                        <option value="Son">Son</option>
                    </select>
                </div>
                <div className="form-group mb-3">
                    <div className="row">
                        <div className="col">
                            <label htmlFor="firstName">First Name</label>
                            <input type="text" className="form-control" id="firstName" placeholder="First Name" value={formData.firstName} onChange={handleChange} />
                            {errors.firstName && <small className="text-danger">{errors.firstName}</small>}
                        </div>
                        <div className="col">
                            <label htmlFor="lastName">Last Name</label>
                            <input type="text" className="form-control" id="lastName" placeholder="Last Name" value={formData.lastName} onChange={handleChange} />
                            {errors.lastName && <small className="text-danger">{errors.lastName}</small>}
                        </div>
                    </div>
                </div>
                <div className="form-group mb-3">
                    <div className="row">
                        <div className="col">
                            <label htmlFor="gender">Gender</label>
                            <select className="form-control" id="gender" value={formData.gender} onChange={handleChange}>
                                <option value="Male">Male</option>
                                <option value="Female">Female</option>
                            </select>
                        </div>

                        <div className="col">
                            <label htmlFor="dob">Date of Birth</label>
                            <input type="date" className="form-control" id="dob" value={formData.dob} onChange={handleChange} />
                            {errors.dob && <small className="text-danger">{errors.dob}</small>}
                        </div>
                    </div>
                </div>
                <div className="form-group mb-3">
                    <label htmlFor="email">Email address</label>
                    <input type="email" className="form-control" id="email" placeholder="Enter your email" value={formData.email} onChange={handleChange} />
                    {errors.email && <small className="text-danger">{errors.email}</small>}
                </div>
                <div className="form-group mb-3">
                    <label htmlFor="phone">Phone</label>
                    <input type="tel" className="form-control" id="phone" placeholder="Enter your phone number" value={formData.phone} onChange={handleChange} />
                    {errors.phone && <small className="text-danger">{errors.phone}</small>}
                </div>
                <div className="form-group mb-3">
                    <div className="row">
                        <div className="col">
                            <label htmlFor="password">Password</label>
                            <input type="password" className="form-control" id="password" placeholder="Enter your password" value={formData.password} onChange={handleChange} />
                            {errors.password && <small className="text-danger">{errors.password}</small>}
                        </div>
                        <div className="col">
                            <label htmlFor="confirmPassword">Confirm Password</label>
                            <input type="password" className="form-control" id="confirmPassword" placeholder="Confirm your password" value={formData.confirmPassword} onChange={handleChange} />
                            {errors.confirmPassword && <small className="text-danger">{errors.confirmPassword}</small>}
                        </div>
                    </div>
                </div>
                <button type="submit" className="btn btn-primary btn-block w-100">Create Account</button>
            </form>
        </div>
    );
};

export default RegistrationForm;
