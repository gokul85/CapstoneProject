import React from 'react';
import { Link } from 'react-router-dom';

const Header = () => {
    return (
        <div className="position-fixed w-100 top-0 z-2">
            <header className="shadow-sm bg-white border-bottom">
                <div className="navbar navbar-expand-lg">
                    <div className="container">
                        <Link className="navbar-brand py-2" to="/">
                            <img src="" alt="Matrimonial App Logo" height="40" />
                        </Link>
                        <ul className="navbar-nav ml-auto d-flex align-items-center justify-content-evenly w-100 flex-row">
                            <li className="nav-item">
                                <Link className="nav-link text-uppercase fw-bold text-primary" to='/#home'>Home</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link text-uppercase fw-bold text-primary" to='/#happystories'>Happy Stories</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link text-uppercase fw-bold text-primary" to='/#contact'>Contact Us</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link text-uppercase fw-bold text-primary" to="/login">Login</Link>
                            </li>
                        </ul>
                    </div>
                </div>
            </header>
        </div>
    );
};

export default Header;
