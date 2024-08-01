import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosInstance from "../../Utils/axiosInstance";
import { toast } from "react-toastify";
import "./SearchPage.css"
import UserHeader from "../../Components/UserHeader"

const SearchPage = () => {
    const navigate = useNavigate();

    const [searchResults, setSearchResults] = useState([]);
    const [paginatedResults, setPaginatedResults] = useState([]);
    const [sidebarOpen, setSidebarOpen] = useState(false);
    const [searchCriteria, setSearchCriteria] = useState({
        minHeight: 0,
        maxHeight: 0,
        minWeight: 0,
        maxWeight: 0,
        maritalStatus: '',
        religion: '',
        language: '',
        profession: '',
        smokeAcceptable: false,
        drinkAcceptable: false,
        state: '',
        complexion: '',
        ageRange: '',
        pp: false
    });
    const [currentPage, setCurrentPage] = useState(1);
    const resultsPerPage = 12;

    useEffect(() => {
        const checkProfileStatus = async () => {
            var token = localStorage.getItem("token");
            if (token == null || token === "") {
                navigate("/login");
            } else {
                try {
                    var res = await axiosInstance.post("/user/verifyprofilestatus");
                    if (!res.data.result) {
                        toast.warn("Please Complete your profile");
                        navigate("/addprofile");
                    }
                } catch (error) {
                    console.error("Error verifying profile status:", error);
                }
            }
        };

        const getProfiles = async () => {
            try {
                console.log(searchCriteria)
                var res = await axiosInstance.post("/search/searchprofiles", searchCriteria);
                if (!res.data.hasError && res.data.result !== null) {
                    setSearchResults(res.data.result.searchProfiles);
                    console.log(res.data.result)
                }
            } catch (error) {
                console.error("Error fetching profiles:", error);
            }
        };

        checkProfileStatus();
        getProfiles();
    }, [searchCriteria, navigate]);


    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setSearchCriteria((prevCriteria) => ({
            ...prevCriteria,
            [name]: value,
            pp: false
        }));
    };


    const toggleSidebar = () => {
        setSidebarOpen(!sidebarOpen);
    };

    const clearFilters = () => {
        setSearchCriteria({
            minHeight: 0,
            maxHeight: 0,
            minWeight: 0,
            maxWeight: 0,
            maritalStatus: '',
            religion: '',
            language: '',
            profession: '',
            smokeAcceptable: false,
            drinkAcceptable: false,
            state: '',
            complexion: '',
            ageRange: '',
            pp: false
        });
    };

    useEffect(() => {
        setPaginatedResults(searchResults.slice((currentPage - 1) * resultsPerPage, currentPage * resultsPerPage));
    }, [searchResults, currentPage]);

    return (
        <div className="search-page container-fluid">
            <UserHeader />

            <div className="row">
                <button className="toggle-button btn btn-primary d-lg-none" onClick={toggleSidebar}>
                    ☰
                </button>
                <div className={`sidebar col-lg-3 col-md-4 ${sidebarOpen ? 'open' : ''}`}>
                    <h4 className='text-primary'>Partner Preferences</h4>
                    <div className="scrollable">
                        <div>
                            <h6>Height Range (cm)</h6>
                            <div className="row w-100">
                                <div className="col-6 mb-3">
                                    <label htmlFor="minHeight" className="form-label">Min Height</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        id="minHeight"
                                        name="minHeight"
                                        value={searchCriteria.minHeight}
                                        onChange={handleInputChange}
                                    />
                                </div>
                                <div className="col-6 mb-3">
                                    <label htmlFor="maxHeight" className="form-label">Max Height</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        id="maxHeight"
                                        name="maxHeight"
                                        value={searchCriteria.maxHeight}
                                        onChange={handleInputChange}
                                    />
                                </div>
                            </div>
                        </div>
                        <div>
                            <h6>Weight Range (kg)</h6>
                            <div className="row w-100">
                                <div className="col-6 mb-3">
                                    <label htmlFor="minWeight" className="form-label">Min Weight</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        id="minWeight"
                                        name="minWeight"
                                        value={searchCriteria.minWeight}
                                        onChange={handleInputChange}
                                    />
                                </div>
                                <div className="col-6 mb-3">
                                    <label htmlFor="maxWeight" className="form-label">Max Weight</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        id="maxWeight"
                                        name="maxWeight"
                                        value={searchCriteria.maxWeight}
                                        onChange={handleInputChange}
                                    />
                                </div>
                            </div>
                        </div>
                        <div>
                            <h6>Marital Status</h6>
                            <select
                                className="form-select"
                                name="maritalStatus"
                                value={searchCriteria.maritalStatus}
                                onChange={handleInputChange}
                            >
                                <option value="">Select</option>
                                <option value="Single">Single</option>
                                <option value="Divorced">Divorced</option>
                                <option value="Widowed">Widowed</option>
                            </select>
                        </div>
                        <div>
                            <h6>Religion</h6>
                            <select
                                className="form-select"
                                name="religion"
                                value={searchCriteria.religion}
                                onChange={handleInputChange}
                            >
                                <option value="">Select</option>
                                <option value="Hinduism">Hinduism</option>
                                <option value="Islam">Islam</option>
                                <option value="Christianity">Christianity</option>
                                <option value="Sikhism">Sikhism</option>
                                <option value="Buddhism">Buddhism</option>
                                <option value="Jainism">Jainism</option>
                            </select>
                        </div>
                        <div>
                            <h6>Language</h6>
                            <select
                                className="form-select"
                                name="language"
                                value={searchCriteria.language}
                                onChange={handleInputChange}
                            >
                                <option value="">Select</option>
                                <option value="Tamil">Tamil</option>
                                <option value="Telugu">Telugu</option>
                                <option value="Hindi">Hindi</option>
                                <option value="Bengali">Bengali</option>
                                <option value="Marathi">Marathi</option>
                                <option value="Gujarati">Gujarati</option>
                                <option value="Kannada">Kannada</option>
                                <option value="Odia">Odia</option>
                                <option value="Malayalam">Malayalam</option>
                            </select>
                        </div>
                        {/* <div>
                            <h6>Profession</h6>
                            <div className="mb-3">
                                <input
                                    type="text"
                                    className="form-control"
                                    name="profession"
                                    value={searchCriteria.profession}
                                    onChange={handleInputChange}
                                />
                            </div>
                        </div> */}
                        <div>
                            <h6>Smoking</h6>
                            <div className="form-check">
                                <input
                                    type="checkbox"
                                    className="form-check-input"
                                    id="smokeAcceptable"
                                    name="smokeAcceptable"
                                    checked={searchCriteria.smokeAcceptable}
                                    onChange={(e) => setSearchCriteria(prev => ({ ...prev, smokeAcceptable: e.target.checked }))}
                                />
                                <label htmlFor="smokeAcceptable" className="form-check-label">Acceptable</label>
                            </div>
                        </div>
                        <div>
                            <h6>Drinking</h6>
                            <div className="form-check">
                                <input
                                    type="checkbox"
                                    className="form-check-input"
                                    id="drinkAcceptable"
                                    name="drinkAcceptable"
                                    checked={searchCriteria.drinkAcceptable}
                                    onChange={(e) => setSearchCriteria(prev => ({ ...prev, drinkAcceptable: e.target.checked }))}
                                />
                                <label htmlFor="drinkAcceptable" className="form-check-label">Acceptable</label>
                            </div>
                        </div>
                        <div>
                            <h6>State</h6>
                            <select
                                className="form-select"
                                name="state"
                                value={searchCriteria.state}
                                onChange={handleInputChange}
                            >
                                <option value="">Select</option>
                                <option value="Tamil Nadu">Tamil Nadu</option>
                                <option value="Andhra Pradesh">Andhra Pradesh</option>
                                <option value="Kerala">Kerala</option>
                                <option value="Karnataka">Karnataka</option>
                                <option value="Maharashtra">Maharashtra</option>
                                <option value="Gujarat">Gujarat</option>
                            </select>
                        </div>
                        <div>
                            <h6>Complexion</h6>
                            <select
                                className="form-select"
                                name="complexion"
                                value={searchCriteria.complexion}
                                onChange={handleInputChange}
                            >
                                <option value="">Select</option>
                                <option value="Fair">Fair</option>
                                <option value="Medium">Medium</option>
                                <option value="Dark">Dark</option>
                            </select>
                        </div>
                        {/* <div>
                            <h6>Age Range</h6>
                            <select
                                className="form-select"
                                name="ageRange"
                                value={searchCriteria.ageRange}
                                onChange={handleInputChange}
                            >
                                <option value="">All</option>
                                <option value="20-29">20-29</option>
                                <option value="30-39">30-39</option>
                                <option value="40-49">40-49</option>
                                <option value="50+">50+</option>
                            </select>
                        </div> */}
                        <div className="mt-3">
                            <button className="btn btn-secondary" onClick={clearFilters}>Clear Filters</button>
                        </div>
                    </div>
                </div>

                <div className="search-results col-lg-9">
                    <div className="row">
                        {paginatedResults.map(profile => (
                            <div key={profile.id} className="col-md-4 mb-4">
                                <div className="card" onClick={() => navigate(`/profile/${profile.id}`)} style={{ cursor: 'pointer' }}>
                                    <img src={profile.image} className="card-img-top" alt={`${profile.name}'s profile`} />
                                    <div className="card-body">
                                        <h5 className="card-title">
                                            <div className="d-flex justify-content-between">
                                                <p>{profile.name}</p>
                                                <p>{profile.age}</p>
                                            </div>
                                        </h5>
                                        <div className="card-text"><strong>Profession:</strong> {profile.profession}</div>
                                        <div className="card-text"><strong>State:</strong> {profile.state}</div>
                                        <div className="badges d-flex justify-content-around my-2">
                                            <span className="badge bg-primary">{profile.maritalStatus}</span>
                                            <span className="badge bg-primary">{profile.religion}</span>
                                            <span className="badge bg-primary">{profile.height}</span>
                                            <span className="badge bg-primary">{profile.weight}</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                    <nav className="d-flex justify-content-center">
                        <ul className="pagination">
                            <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={() => setCurrentPage(1)}>First</button>
                            </li>
                            <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={() => setCurrentPage(currentPage - 1)}>Previous</button>
                            </li>
                            {Array.from({ length: 5 }, (_, index) => {
                                const page = Math.max(1, currentPage - 2) + index;
                                return (
                                    page <= Math.ceil(searchResults.length / resultsPerPage) && (
                                        <li key={page} className={`page-item ${currentPage === page ? 'active' : ''}`}>
                                            <button className="page-link" onClick={() => setCurrentPage(page)}>{page}</button>
                                        </li>
                                    )
                                );
                            })}
                            <li className={`page-item ${currentPage === Math.ceil(searchResults.length / resultsPerPage) ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={() => setCurrentPage(currentPage + 1)}>Next</button>
                            </li>
                            <li className={`page-item ${currentPage === Math.ceil(searchResults.length / resultsPerPage) ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={() => setCurrentPage(Math.ceil(searchResults.length / resultsPerPage))}>Last</button>
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
        </div>
    );
};

export default SearchPage;