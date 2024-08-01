import React, { useEffect } from "react";
import Banner from "./Banner";
import Header from "./Header";
import HowItWorks from "./HowItWorks";
import { useNavigate } from "react-router-dom";

const Home = () => {
    const navigate = useNavigate();

    useEffect(() => {
        var token = localStorage.getItem("token");
        if (token != null) {
            navigate("/search")
        }
    })

    return (
        <div>
            <Header />
            <Banner />
            <HowItWorks />
        </div>
    );
}

export default Home;