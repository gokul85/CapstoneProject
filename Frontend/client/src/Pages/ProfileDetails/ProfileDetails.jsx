import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

const AddProfileDetails = () => {
    const [step, setStep] = useState(1);
    const [formData, setFormData] = useState({
        profileImage: '',
        bio: '',
        nativeLanguage: 'Tamil',
        maritalStatus: 'Single',
        religion: 'Hinduism',
        caste: '',
        highestQualification: '',
        addressLine: 'Test Address',
        city: 'Test City',
        state: 'Tamil Nadu',
        pincode: '620001',
        height: '175',
        weight: '65',
        eyeColor: 'Black',
        hairColor: 'Black',
        complexion: 'Medium',
        bloodGroup: 'A+',
        disability: 'No',
        disabilityDetails: '',
        father: 'Alive',
        mother: 'Alive',
        siblingsCount: 2,
        drink: 'No',
        smoke: 'No',
        livingWith: 'With Family',
        galleryImages: [],
        education: [{
            degree: 'BE',
            specialization: 'CSE',
            startYear: 2020,
            endYear: 2024,
            status: 'Completed'
        }],
        career: [{
            designation: 'Associate Engineer',
            company: 'Test Company',
            startYear: 2024,
            endYear: null,
        }]
    });

    const handleEducationChange = (index, e) => {
        const { name, value } = e.target;
        const updatedEducation = [...formData.education];
        updatedEducation[index] = { ...updatedEducation[index], [name]: value };
        setFormData({ ...formData, education: updatedEducation });
    };

    const handleCareerChange = (index, e) => {
        const { name, value } = e.target;
        setFormData(prevState => {
            const newCareer = [...prevState.career];
            if (name === 'endYear' && value === '') {
                newCareer[index].endYear = null;
            } else {
                newCareer[index][name] = value;
            }
            return { ...prevState, career: newCareer };
        });
    };

    const addEducation = () => {
        setFormData({
            ...formData,
            education: [...formData.education, {
                degree: '',
                specialization: '',
                startDate: '',
                endDate: '',
                status: ''
            }]
        });
    };

    const removeEducation = (index) => {
        const updatedEducation = formData.education.filter((_, i) => i !== index);
        setFormData({ ...formData, education: updatedEducation });
    };

    const addCareer = () => {
        setFormData({
            ...formData,
            career: [...formData.career, {
                designation: '',
                company: '',
                startDate: '',
                endDate: null,
            }]
        });
    };

    const removeCareer = (index) => {
        const updatedCareer = formData.career.filter((_, i) => i !== index);
        setFormData({ ...formData, career: updatedCareer });
    };


    const handleChange = (e) => {
        const { name, value, files } = e.target;
        setFormData({
            ...formData,
            [name]: files ? files[0] : value,
        });
    };

    const validateStep = () => {
        const requiredFields = {
            1: ['profileImage'],
            2: ['nativeLanguage', 'maritalStatus', 'religion', 'highestQualification'],
            3: ['addressLine', 'city', 'state', 'pincode'],
            4: ['height', 'weight', 'eyeColor', 'hairColor', 'complexion', 'bloodGroup', 'disability'],
            5: ['father', 'mother', 'siblingsCount'],
            6: ['drink', 'smoke']
        };

        return requiredFields[step].every(field => formData[field] !== '');
    };

    const validateEducation = () => {
        return formData.education.every(edu =>
            edu.degree && edu.specialization && edu.startYear && edu.endYear && edu.status
        );
    };

    const validateCareer = () => {
        return formData.career.every(car =>
            car.designation && car.company && car.startYear && (car.endYear || car.endYear === null)
        );
    };


    const handleNext = () => {
        if (step === 7) {
            if (validateEducation()) {
                setStep(step + 1);
            }
        }
        else if (validateStep()) {
            setStep(step + 1);
        } else {
            alert('Please fill all required fields.');
        }
    };

    const handlePrevious = () => {
        setStep(step - 1);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (validateCareer()) {
            // Submit form data
            console.log(formData);
        } else {
            alert('Please fill all required fields.');
        }
    };

    const commonLanguages = ["Tamil", "Telugu", "Hindi", "Bengali", "Marathi", "Gujarati", "Kannada", "Odia", "Malayalam"];
    const commonReligions = ["Hinduism", "Islam", "Christianity", "Sikhism", "Buddhism", "Jainism"];
    const statesOfIndia = ["Andhra Pradesh", "Arunachal Pradesh", "Assam", "Bihar", "Chhattisgarh", "Goa", "Gujarat", "Haryana", "Himachal Pradesh", "Jharkhand", "Karnataka", "Kerala", "Madhya Pradesh", "Maharashtra", "Manipur", "Meghalaya", "Mizoram", "Nagaland", "Odisha", "Punjab", "Rajasthan", "Sikkim", "Tamil Nadu", "Telangana", "Tripura", "Uttar Pradesh", "Uttarakhand", "West Bengal"];
    const eyeColors = ["Black", "Brown", "Blue", "Green", "Hazel"];
    const hairColors = ["Black", "Brown", "Blonde", "Grey", "White"];
    const complexions = ["Fair", "Medium", "Olive", "Brown", "Dark"];
    const bloodGroups = ["A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-"];
    const livingWithOptions = ["Alone", "With Family"];

    return (
        <div className="container mt-5">
            <div className="row justify-content-center">
                <div className="col-md-8">
                    <div className="card">
                        <div className="card-body">
                            <form onSubmit={handleSubmit}>
                                {step === 1 && (
                                    <>
                                        <h2>Profile Image</h2>
                                        <div className="mb-3">
                                            <label htmlFor="profileImage" className="form-label">Profile Image</label>
                                            <input
                                                type="file"
                                                className="form-control"
                                                id="profileImage"
                                                name="profileImage"
                                                onChange={handleChange}
                                                required
                                            />
                                        </div>
                                        <button className="btn btn-primary" type="button" onClick={handleNext}>Next</button>
                                    </>
                                )}

                                {step === 2 && (
                                    <>
                                        <h2>Basic Info</h2>
                                        <div className="mb-3">
                                            <label htmlFor="bio" className="form-label">Intro or Bio</label>
                                            <textarea
                                                className="form-control"
                                                id="bio"
                                                name="bio"
                                                value={formData.bio}
                                                onChange={handleChange}
                                            />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="nativeLanguage" className="form-label">Native Language</label>
                                            <select
                                                className="form-control"
                                                id="nativeLanguage"
                                                name="nativeLanguage"
                                                value={formData.nativeLanguage}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Native Language</option>
                                                {commonLanguages.map(lang => (
                                                    <option key={lang} value={lang}>{lang}</option>
                                                ))}
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="maritalStatus" className="form-label">Marital Status</label>
                                            <select
                                                className="form-control"
                                                id="maritalStatus"
                                                name="maritalStatus"
                                                value={formData.maritalStatus}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Marital Status</option>
                                                <option value="Single">Single</option>
                                                <option value="Married">Married</option>
                                                <option value="Divorced">Divorced</option>
                                                <option value="Widowed">Widowed</option>
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="religion" className="form-label">Religion</label>
                                            <select
                                                className="form-control"
                                                id="religion"
                                                name="religion"
                                                value={formData.religion}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Religion</option>
                                                {commonReligions.map(religion => (
                                                    <option key={religion} value={religion}>{religion}</option>
                                                ))}
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="caste" className="form-label">Caste</label>
                                            <input
                                                type="text"
                                                className="form-control"
                                                id="caste"
                                                name="caste"
                                                value={formData.caste}
                                                onChange={handleChange}
                                            />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="highestQualification" className="form-label">Highest Qualification</label>
                                            <select
                                                className="form-control"
                                                id="highestQualification"
                                                name="highestQualification"
                                                value={formData.highestQualification}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Your Highest Qualification</option>
                                                <option value="No Formal Education">No Formal Education</option>
                                                <option value="High School / Secondary School">High School / Secondary School</option>
                                                <option value="Associate Degree / Diploma">Associate Degree / Diploma</option>
                                                <option value="Bachelor's Degree">Bachelor's Degree</option>
                                                <option value="Master's Degree">Master's Degree</option>
                                                <option value="Doctorate / PhD">Doctorate / PhD</option>
                                                <option value="Post-Doctoral Research">Post-Doctoral Research</option>
                                            </select>
                                        </div>
                                        <button className="btn btn-secondary" type="button" onClick={handlePrevious}>Previous</button>
                                        <button className="btn btn-primary" type="button" onClick={handleNext}>Next</button>
                                    </>
                                )}

                                {step === 3 && (
                                    <>
                                        <h2>Address</h2>
                                        <div className="mb-3">
                                            <label htmlFor="addressLine" className="form-label">Address Line</label>
                                            <input
                                                type="text"
                                                className="form-control"
                                                id="addressLine"
                                                name="addressLine"
                                                value={formData.addressLine}
                                                onChange={handleChange}
                                                required
                                            />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="city" className="form-label">City</label>
                                            <input
                                                type="text"
                                                className="form-control"
                                                id="city"
                                                name="city"
                                                value={formData.city}
                                                onChange={handleChange}
                                                required
                                            />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="state" className="form-label">State</label>
                                            <select
                                                className="form-control"
                                                id="state"
                                                name="state"
                                                value={formData.state}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select State</option>
                                                {statesOfIndia.map(state => (
                                                    <option key={state} value={state}>{state}</option>
                                                ))}
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="pincode" className="form-label">Pincode</label>
                                            <input
                                                type="text"
                                                className="form-control"
                                                id="pincode"
                                                name="pincode"
                                                value={formData.pincode}
                                                onChange={handleChange}
                                                required
                                            />
                                        </div>
                                        <button className="btn btn-secondary" type="button" onClick={handlePrevious}>Previous</button>
                                        <button className="btn btn-primary" type="button" onClick={handleNext}>Next</button>
                                    </>
                                )}

                                {step === 4 && (
                                    <>
                                        <h2>Physical Attributes</h2>
                                        <div className="mb-3">
                                            <label htmlFor="height" className="form-label">Height (cm)</label>
                                            <input
                                                type="text"
                                                className="form-control"
                                                id="height"
                                                name="height"
                                                value={formData.height}
                                                onChange={handleChange}
                                                required
                                            />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="weight" className="form-label">Weight (Kg)</label>
                                            <input
                                                type="text"
                                                className="form-control"
                                                id="weight"
                                                name="weight"
                                                value={formData.weight}
                                                onChange={handleChange}
                                                required
                                            />
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="eyeColor" className="form-label">Eye Color</label>
                                            <select
                                                className="form-control"
                                                id="eyeColor"
                                                name="eyeColor"
                                                value={formData.eyeColor}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Eye Color</option>
                                                {eyeColors.map(color => (
                                                    <option key={color} value={color}>{color}</option>
                                                ))}
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="hairColor" className="form-label">Hair Color</label>
                                            <select
                                                className="form-control"
                                                id="hairColor"
                                                name="hairColor"
                                                value={formData.hairColor}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Hair Color</option>
                                                {hairColors.map(color => (
                                                    <option key={color} value={color}>{color}</option>
                                                ))}
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="complexion" className="form-label">Complexion</label>
                                            <select
                                                className="form-control"
                                                id="complexion"
                                                name="complexion"
                                                value={formData.complexion}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Complexion</option>
                                                {complexions.map(complexion => (
                                                    <option key={complexion} value={complexion}>{complexion}</option>
                                                ))}
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="bloodGroup" className="form-label">Blood Group</label>
                                            <select
                                                className="form-control"
                                                id="bloodGroup"
                                                name="bloodGroup"
                                                value={formData.bloodGroup}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Blood Group</option>
                                                {bloodGroups.map(group => (
                                                    <option key={group} value={group}>{group}</option>
                                                ))}
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="disability" className="form-label">Disability</label>
                                            <select
                                                className="form-control"
                                                id="disability"
                                                name="disability"
                                                value={formData.disability}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Disability</option>
                                                <option value="No">No</option>
                                                <option value="Yes">Yes</option>
                                            </select>
                                            {formData.disability === 'Yes' && (
                                                <div className="mt-3">
                                                    <label htmlFor="disabilityDetails" className="form-label">Disability Details</label>
                                                    <input
                                                        type="text"
                                                        className="form-control"
                                                        id="disabilityDetails"
                                                        name="disabilityDetails"
                                                        value={formData.disabilityDetails}
                                                        onChange={handleChange}
                                                        required
                                                    />
                                                </div>
                                            )}
                                        </div>
                                        <button className="btn btn-secondary" type="button" onClick={handlePrevious}>Previous</button>
                                        <button className="btn btn-primary" type="button" onClick={handleNext}>Next</button>
                                    </>
                                )}

                                {step === 5 && (
                                    <>
                                        <h2>Family Info</h2>
                                        <div className="mb-3">
                                            <label htmlFor="father" className="form-label">Father</label>
                                            <select
                                                className="form-control"
                                                id="father"
                                                name="father"
                                                value={formData.father}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Father's Status</option>
                                                <option value="Alive">Alive</option>
                                                <option value="Deceased">Deceased</option>
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="mother" className="form-label">Mother</label>
                                            <select
                                                className="form-control"
                                                id="mother"
                                                name="mother"
                                                value={formData.mother}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Mother's Status</option>
                                                <option value="Alive">Alive</option>
                                                <option value="Deceased">Deceased</option>
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="siblingsCount" className="form-label">Number of Siblings</label>
                                            <input
                                                type="number"
                                                className="form-control"
                                                id="siblingsCount"
                                                name="siblingsCount"
                                                value={formData.siblingsCount}
                                                onChange={handleChange}
                                                required
                                            />
                                        </div>
                                        <button className="btn btn-secondary" type="button" onClick={handlePrevious}>Previous</button>
                                        <button className="btn btn-primary" type="button" onClick={handleNext}>Next</button>
                                    </>
                                )}

                                {step === 6 && (
                                    <>
                                        <h2>Lifestyle</h2>
                                        <div className="mb-3">
                                            <label htmlFor="drink" className="form-label">Do you drink?</label>
                                            <select
                                                className="form-control"
                                                id="drink"
                                                name="drink"
                                                value={formData.drink}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Option</option>
                                                <option value="Yes">Yes</option>
                                                <option value="No">No</option>
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="smoke" className="form-label">Do you smoke?</label>
                                            <select
                                                className="form-control"
                                                id="smoke"
                                                name="smoke"
                                                value={formData.smoke}
                                                onChange={handleChange}
                                                required
                                            >
                                                <option value="">Select Option</option>
                                                <option value="Yes">Yes</option>
                                                <option value="No">No</option>
                                            </select>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="livingWith" className="form-label">Living With</label>
                                            <select
                                                className="form-control"
                                                id="livingWith"
                                                name="livingWith"
                                                value={formData.livingWith}
                                                onChange={handleChange}
                                            >
                                                <option value="">Select Option</option>
                                                {livingWithOptions.map(option => (
                                                    <option key={option} value={option}>{option}</option>
                                                ))}
                                            </select>
                                        </div>
                                        <button className="btn btn-secondary" type="button" onClick={handlePrevious}>Previous</button>
                                        <button className="btn btn-primary" type="button" onClick={handleNext}>Next</button>
                                    </>
                                )}

                                {step === 7 && (
                                    <>
                                        <h2>Education</h2>
                                        {formData.education.map((edu, index) => (
                                            <div key={index} className="education-entry">
                                                <div className="mb-3">
                                                    <label htmlFor={`degree-${index}`} className="form-label">Degree</label>
                                                    <input
                                                        type="text"
                                                        className="form-control"
                                                        id={`degree-${index}`}
                                                        name="degree"
                                                        value={edu.degree}
                                                        onChange={(e) => handleEducationChange(index, e)}
                                                        required
                                                    />
                                                </div>
                                                <div className="mb-3">
                                                    <label htmlFor={`specialization-${index}`} className="form-label">Specialization</label>
                                                    <input
                                                        type="text"
                                                        className="form-control"
                                                        id={`specialization-${index}`}
                                                        name="specialization"
                                                        value={edu.specialization}
                                                        onChange={(e) => handleEducationChange(index, e)}
                                                        required
                                                    />
                                                </div>
                                                <div className="mb-3">
                                                    <label htmlFor={`startYearEducation-${index}`} className="form-label">Start Year</label>
                                                    <input
                                                        type="number"
                                                        min="1900"
                                                        max="2100"
                                                        className="form-control"
                                                        id={`startYearEducation-${index}`}
                                                        name="startYear"
                                                        value={edu.startYear}
                                                        onChange={(e) => handleEducationChange(index, e)}
                                                        required
                                                    />
                                                </div>
                                                <div className="mb-3">
                                                    <label htmlFor={`endYearEducation-${index}`} className="form-label">End Year</label>
                                                    <input
                                                        type="number"
                                                        min="1900"
                                                        max="2100"
                                                        className="form-control"
                                                        id={`endYearEducation-${index}`}
                                                        name="endYear"
                                                        value={edu.endYear}
                                                        onChange={(e) => handleEducationChange(index, e)}
                                                        required
                                                    />
                                                </div>
                                                <div className="mb-3">
                                                    <label htmlFor={`statusEducation-${index}`} className="form-label">Status</label>
                                                    <select
                                                        className="form-control"
                                                        id={`statusEducation-${index}`}
                                                        name="status"
                                                        value={edu.status}
                                                        onChange={(e) => handleEducationChange(index, e)}
                                                        required
                                                    >
                                                        <option value="">Select Status</option>
                                                        <option value="Completed">Completed</option>
                                                        <option value="Ongoing">Ongoing</option>
                                                    </select>
                                                </div>
                                                <button type="button" className="btn btn-danger" onClick={() => removeEducation(index)}>Remove</button>
                                            </div>
                                        ))}
                                        <button type="button" className="btn btn-primary" onClick={addEducation}>Add Education</button>
                                        <button className="btn btn-secondary" type="button" onClick={handlePrevious}>Previous</button>
                                        <button className="btn btn-primary" type="button" onClick={handleNext}>Next</button>
                                    </>
                                )}


                                {step === 8 && (
                                    <>
                                        <h2>Career</h2>
                                        {formData.career.map((car, index) => (
                                            <div key={index} className="career-entry">
                                                <div className="mb-3">
                                                    <label htmlFor={`designation-${index}`} className="form-label">Designation</label>
                                                    <input
                                                        type="text"
                                                        className="form-control"
                                                        id={`designation-${index}`}
                                                        name="designation"
                                                        value={car.designation}
                                                        onChange={(e) => handleCareerChange(index, e)}
                                                        required
                                                    />
                                                </div>
                                                <div className="mb-3">
                                                    <label htmlFor={`company-${index}`} className="form-label">Company</label>
                                                    <input
                                                        type="text"
                                                        className="form-control"
                                                        id={`company-${index}`}
                                                        name="company"
                                                        value={car.company}
                                                        onChange={(e) => handleCareerChange(index, e)}
                                                        required
                                                    />
                                                </div>
                                                <div className="mb-3">
                                                    <label htmlFor={`startYearCareer-${index}`} className="form-label">Start Year</label>
                                                    <input
                                                        type="number"
                                                        min="1900"
                                                        max="2100"
                                                        className="form-control"
                                                        id={`startYearCareer-${index}`}
                                                        name="startYear"
                                                        value={car.startYear}
                                                        onChange={(e) => handleCareerChange(index, e)}
                                                        required
                                                    />
                                                </div>
                                                <div className="mb-3">
                                                    <label htmlFor={`endYearCareer-${index}`} className="form-label">End Year</label>
                                                    <input
                                                        type="number"
                                                        name="endYear"
                                                        value={car.endYear || ''}
                                                        onChange={(e) => handleCareerChange(index, e)}
                                                        placeholder="Leave empty if current"
                                                        min="1900"
                                                        max="2100"
                                                        className="form-control"
                                                        id={`endYearCareer-${index}`}
                                                    />
                                                </div>
                                                <button type="button" className="btn btn-danger" onClick={() => removeCareer(index)}>Remove</button>
                                            </div>
                                        ))}
                                        <button type="button" className="btn btn-primary" onClick={addCareer}>Add Career</button>
                                        <button className="btn btn-secondary" type="button" onClick={handlePrevious}>Previous</button>
                                        <button className="btn btn-success" type="submit">Submit</button>
                                    </>
                                )}
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AddProfileDetails;
