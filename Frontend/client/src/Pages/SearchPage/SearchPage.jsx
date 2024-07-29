import React, { useState } from 'react';
import './SearchPage.css'; // Ensure you create and import this CSS file for styling

const SearchPage = () => {
    const [sidebarOpen, setSidebarOpen] = useState(false);
    const [searchCriteria, setSearchCriteria] = useState({
        heightRange: [],
        weightRange: [],
        maritalStatus: [],
        religion: [],
        language: [],
        education: [],
        profession: [],
        smokeAcceptable: false,
        drinkAcceptable: false,
        state: [],
        complexion: [],
    });

    const handleCheckboxChange = (category, value) => {
        setSearchCriteria((prevCriteria) => {
            const newCategoryValues = prevCriteria[category].includes(value)
                ? prevCriteria[category].filter((item) => item !== value)
                : [...prevCriteria[category], value];
            return { ...prevCriteria, [category]: newCategoryValues };
        });
        console.log(searchCriteria);
    };

    const toggleSidebar = () => {
        setSidebarOpen(!sidebarOpen);
    };

    return (
        <div className="search-page">
            <button className="toggle-button" onClick={toggleSidebar}>
                ☰
            </button>
            <div className={`sidebar ${sidebarOpen ? 'open' : ''}`}>
                <h3>Partner Preferences</h3>
                <div className="scrollable">
                    <div>
                        <h4>Height Range (cm)</h4>
                        {[120, 130, 140, 150, 160, 170, 180, 190, 200].map((range) => (
                            <div key={range}>
                                <input
                                    type="checkbox"
                                    id={`height-${range}`}
                                    onChange={() => handleCheckboxChange('heightRange', range)}
                                />
                                <label htmlFor={`height-${range}`}>{range}-{range + 10}</label>
                            </div>
                        ))}
                    </div>
                    <div>
                        <h4>Weight Range (kg)</h4>
                        {[40, 50, 60, 70, 80, 90, 100].map((range) => (
                            <div key={range}>
                                <input
                                    type="checkbox"
                                    id={`weight-${range}`}
                                    onChange={() => handleCheckboxChange('weightRange', range)}
                                />
                                <label htmlFor={`weight-${range}`}>{range}-{range + 10}</label>
                            </div>
                        ))}
                    </div>
                    <div>
                        <h4>Marital Status</h4>
                        {["Single", "Divorced", "Widowed"].map((status) => (
                            <div key={status}>
                                <input
                                    type="checkbox"
                                    id={`marital-${status}`}
                                    onChange={() => handleCheckboxChange('maritalStatus', status)}
                                />
                                <label htmlFor={`marital-${status}`}>{status}</label>
                            </div>
                        ))}
                    </div>
                    <div>
                        <h4>Religion</h4>
                        {["Hinduism", "Islam", "Christianity", "Sikhism", "Buddhism", "Jainism"].map((religion) => (
                            <div key={religion}>
                                <input
                                    type="checkbox"
                                    id={`religion-${religion}`}
                                    onChange={() => handleCheckboxChange('religion', religion)}
                                />
                                <label htmlFor={`religion-${religion}`}>{religion}</label>
                            </div>
                        ))}
                    </div>
                    <div>
                        <h4>Language</h4>
                        {["Tamil", "Telugu", "Hindi", "Bengali", "Marathi", "Gujarati", "Kannada", "Odia", "Malayalam"].map((language) => (
                            <div key={language}>
                                <input
                                    type="checkbox"
                                    id={`language-${language}`}
                                    onChange={() => handleCheckboxChange('language', language)}
                                />
                                <label htmlFor={`language-${language}`}>{language}</label>
                            </div>
                        ))}
                    </div>
                    <div>
                        <h4>Minimum Education Qualification</h4>
                        {["High School", "Bachelor's", "Master's", "Ph.D."].map((education) => (
                            <div key={education}>
                                <input
                                    type="checkbox"
                                    id={`education-${education}`}
                                    onChange={() => handleCheckboxChange('education', education)}
                                />
                                <label htmlFor={`education-${education}`}>{education}</label>
                            </div>
                        ))}
                    </div>
                    <div>
                        <h4>Profession</h4>
                        {["Engineer", "Doctor", "Teacher", "Business", "Others"].map((profession) => (
                            <div key={profession}>
                                <input
                                    type="checkbox"
                                    id={`profession-${profession}`}
                                    onChange={() => handleCheckboxChange('profession', profession)}
                                />
                                <label htmlFor={`profession-${profession}`}>{profession}</label>
                            </div>
                        ))}
                    </div>
                    <div>
                        <h4>Smoke Acceptable</h4>
                        <div>
                            <input
                                type="checkbox"
                                id="smokeAcceptable"
                                onChange={() => setSearchCriteria((prev) => ({ ...prev, smokeAcceptable: !prev.smokeAcceptable }))}
                            />
                            <label htmlFor="smokeAcceptable">Smoke Acceptable</label>
                        </div>
                    </div>
                    <div>
                        <h4>Drink Acceptable</h4>
                        <div>
                            <input
                                type="checkbox"
                                id="drinkAcceptable"
                                onChange={() => setSearchCriteria((prev) => ({ ...prev, drinkAcceptable: !prev.drinkAcceptable }))}
                            />
                            <label htmlFor="drinkAcceptable">Drink Acceptable</label>
                        </div>
                    </div>
                    <div>
                        <h4>State</h4>
                        {["Andhra Pradesh", "Arunachal Pradesh", "Assam", "Bihar", "Chhattisgarh", "Goa", "Gujarat", "Haryana", "Himachal Pradesh", "Jharkhand", "Karnataka", "Kerala", "Madhya Pradesh", "Maharashtra", "Manipur", "Meghalaya", "Mizoram", "Nagaland", "Odisha", "Punjab", "Rajasthan", "Sikkim", "Tamil Nadu", "Telangana", "Tripura", "Uttar Pradesh", "Uttarakhand", "West Bengal"].map((state) => (
                            <div key={state}>
                                <input
                                    type="checkbox"
                                    id={`state-${state}`}
                                    onChange={() => handleCheckboxChange('state', state)}
                                />
                                <label htmlFor={`state-${state}`}>{state}</label>
                            </div>
                        ))}
                    </div>
                    <div>
                        <h4>Complexion</h4>
                        {["Fair", "Medium", "Olive", "Brown", "Dark"].map((complexion) => (
                            <div key={complexion}>
                                <input
                                    type="checkbox"
                                    id={`complexion-${complexion}`}
                                    onChange={() => handleCheckboxChange('complexion', complexion)}
                                />
                                <label htmlFor={`complexion-${complexion}`}>{complexion}</label>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
            <div className="profile-section">
                <h2>Profile Details</h2>
                <div className="profile-content">
                    {/* Placeholder content for profile details */}
                    <p>Profile information will be displayed here.</p>
                </div>
            </div>
        </div>
    );
};

export default SearchPage;
