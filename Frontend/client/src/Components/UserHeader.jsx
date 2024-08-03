import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode"

const UserHeader = () => {
    const [isPremium, setIsPremium] = useState(false)
    const navigate = useNavigate();
    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            try {
                const decodedToken = jwtDecode(token);
                setIsPremium(decodedToken.isPremium === "False" ? false : true);
            } catch (error) {
                console.error("Error decoding token", error);
            }
        } else {
            navigate("/login");
        }
    }, [navigate])

    return (
        <header className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                <a className="navbar-brand" href="/">Logo</a>
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav ms-auto">
                        <li className="nav-item text-end">
                            <Link className="nav-link" to="/search">Search</Link>
                        </li>
                        {isPremium === false ? (<li className="nav-item text-end">
                            <Link className="nav-link" to="/premium">Premium</Link>
                        </li>) : (<></>)}
                        <li className="nav-item text-end">
                            <Link className="nav-link" to="/">Profile</Link>
                        </li>
                        <li className="nav-item text-end">
                            <Link className="nav-link" to="/logout">Logout</Link>
                        </li>
                    </ul>
                </div>
            </div>
        </header>
    )
}

export default UserHeader;