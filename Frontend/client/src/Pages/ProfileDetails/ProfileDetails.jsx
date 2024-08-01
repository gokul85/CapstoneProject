import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { toast } from "react-toastify"
import axiosInstance from "../../Utils/axiosInstance"
import { useNavigate } from "react-router-dom"

const AddProfileDetails = () => {

    useEffect(() => {
        const checkProfileStatus = async () => {
            var token = localStorage.getItem("token");
            if (token == null || token === "") {
                navigate("/login");
            } else {
                try {
                    var res = await axiosInstance.post("/user/verifyprofilestatus");
                    if (res.data.result) {
                        toast.warn("Profile Already Completed");
                        navigate("/search");
                    }
                } catch (error) {
                    console.error("Error verifying profile status:", error);
                }
            }
        };

        checkProfileStatus();
    })

    const navigate = useNavigate();
    const [step, setStep] = useState(1);
    const [formData, setFormData] = useState({
        profileImage: '',
        bio: '',
        nativeLanguage: 'Tamil',
        maritalStatus: 'Single',
        religion: 'Hindu',
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
            startYear: 2012,
            endYear: 2016,
            status: 'Completed'
        }],
        career: [{
            designation: 'Associate Engineer',
            company: 'Test Company',
            startYear: 2016,
            endYear: null,
        }],
        partnerPreferences: {
            heightMin: 160,
            heightMax: 170,
            weightMin: 50,
            weightMax: 60,
            maritalStatus: 'Single',
            religion: 'Hindu',
            language: 'Tamil',
            education: '',
            smokeAcceptable: 'No',
            drinkAcceptable: 'No',
            state: 'Tamil Nadu',
            complexion: 'Fair'
        }
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
            6: ['drink', 'smoke'],
            10: ['partnerPreferences.heightMin', 'partnerPreferences.heightMax', 'partnerPreferences.weightMin', 'partnerPreferences.weightMax', 'partnerPreferences.maritalStatus', 'partnerPreferences.religion', 'partnerPreferences.language', 'partnerPreferences.education', 'partnerPreferences.smokeAcceptable', 'partnerPreferences.drinkAcceptable', 'partnerPreferences.state', 'partnerPreferences.complexion']
        };

        if (step in requiredFields) {
            return requiredFields[step].every(field => {
                const [parent, child] = field.split('.');
                return child ? formData[parent][child] !== '' : formData[field] !== '';
            });
        }
        return true;
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
            } else {
                toast.error('Please fill all required fields.');
            }
        } else if (step === 8) {
            if (validateCareer()) {
                setStep(step + 1);
            } else {
                toast.error('Please fill all required fields.');
            }
        }
        else if (validateStep()) {
            setStep(step + 1);
        } else {
            toast.error('Please fill all required fields.');
        }
    };

    const handlePrevious = () => {
        setStep(step - 1);
    };

    const handleGalleryChange = (e) => {
        const files = Array.from(e.target.files);
        setFormData({ ...formData, galleryImages: files });
    };

    const handlePartnerPreferencesChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, partnerPreferences: { ...formData.partnerPreferences, [name]: value } });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (validateStep()) {
            const formDatasend = new FormData();
            formDatasend.append('profileImage', formData.profileImage);
            formDatasend.append('bio', formData.bio);
            formDatasend.append('nativeLanguage', formData.nativeLanguage);
            formDatasend.append('maritalStatus', formData.maritalStatus);
            formDatasend.append('religion', formData.religion);
            formDatasend.append('caste', formData.caste);
            formDatasend.append('highestQualification', formData.highestQualification);
            formDatasend.append('addressLine', formData.addressLine);
            formDatasend.append('city', formData.city);
            formDatasend.append('state', formData.state);
            formDatasend.append('pincode', formData.pincode);
            formDatasend.append('height', formData.height);
            formDatasend.append('weight', formData.weight);
            formDatasend.append('eyeColor', formData.eyeColor);
            formDatasend.append('hairColor', formData.hairColor);
            formDatasend.append('complexion', formData.complexion);
            formDatasend.append('bloodGroup', formData.bloodGroup);
            formDatasend.append('disability', formData.disability);
            formDatasend.append('disabilityDetails', formData.disabilityDetails);
            formDatasend.append('father', formData.father);
            formDatasend.append('mother', formData.mother);
            formDatasend.append('siblingsCount', formData.siblingsCount);
            formDatasend.append('drink', formData.drink);
            formDatasend.append('smoke', formData.smoke);
            formDatasend.append('livingWith', formData.livingWith);

            formData.galleryImages.forEach(file => {
                formDatasend.append('galleryImages', file);
            });

            formData.education.forEach((edu, index) => {
                formDatasend.append(`education[${index}][degree]`, edu.degree);
                formDatasend.append(`education[${index}][specialization]`, edu.specialization);
                formDatasend.append(`education[${index}][startYear]`, edu.startYear);
                formDatasend.append(`education[${index}][endYear]`, edu.endYear);
                formDatasend.append(`education[${index}][status]`, edu.status);
            });

            formData.career.forEach((car, index) => {
                formDatasend.append(`career[${index}][designation]`, car.designation);
                formDatasend.append(`career[${index}][company]`, car.company);
                formDatasend.append(`career[${index}][startYear]`, car.startYear);
                formDatasend.append(`career[${index}][endYear]`, car.endYear == null ? "" : car.endYear);
            });

            formDatasend.append('partnerPreferences[heightMin]', formData.partnerPreferences.heightMin);
            formDatasend.append('partnerPreferences[heightMax]', formData.partnerPreferences.heightMax);
            formDatasend.append('partnerPreferences[weightMin]', formData.partnerPreferences.weightMin);
            formDatasend.append('partnerPreferences[weightMax]', formData.partnerPreferences.weightMax);
            formDatasend.append('partnerPreferences[maritalStatus]', formData.partnerPreferences.maritalStatus);
            formDatasend.append('partnerPreferences[religion]', formData.partnerPreferences.religion);
            formDatasend.append('partnerPreferences[language]', formData.partnerPreferences.language);
            formDatasend.append('partnerPreferences[education]', formData.partnerPreferences.education);
            formDatasend.append('partnerPreferences[smokeAcceptable]', formData.partnerPreferences.smokeAcceptable);
            formDatasend.append('partnerPreferences[drinkAcceptable]', formData.partnerPreferences.drinkAcceptable);
            formDatasend.append('partnerPreferences[state]', formData.partnerPreferences.state);
            formDatasend.append('partnerPreferences[complexion]', formData.partnerPreferences.complexion);
            console.log(formDatasend);
            axiosInstance.post("/profile/addprofile", formDatasend, {
                headers: {
                    "Content-Type": 'multipart/form-data'
                }
            })
                .then(response => {
                    toast.success("Profile Successfully Completed");
                    navigate("/search");
                })
                .catch(err => {
                    toast.error(err.response.data);
                    console.log(err.response.data);
                });
        } else {
            toast.error('Please fill all required fields.');
        }
    };

    const commonLanguages = ["Tamil", "Telugu", "Hindi", "Bengali", "Marathi", "Gujarati", "Kannada", "Odia", "Malayalam"];
    const commonReligions = ["Hindu", "Islam", "Christian", "Sikhism", "Buddhism", "Jainism"];
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
                                        <h2 className='text-primary text-center'>Profile Image</h2>
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
                                        <div className="d-flex justify-content-end">
                                            <button className="btn btn-primary" type="button" onClick={handleNext}>Next</button>
                                        </div>
                                    </>
                                )}

                                {step === 2 && (
                                    <>
                                        <h2 className='text-primary text-center'>Basic Info</h2>
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
                                                <option value="High School">High School</option>
                                                <option value="Diploma">Associate Degree / Diploma</option>
                                                <option value="Bachelor's">Bachelor's Degree</option>
                                                <option value="Master's">Master's Degree</option>
                                                <option value="PhD">Doctorate / PhD</option>
                                            </select>
                                        </div>
                                        <div className="d-flex justify-content-end">
                                            <button className="btn btn-secondary m-2" type="button" onClick={handlePrevious}>Previous</button>
                                            <button className="btn btn-primary m-2" type="button" onClick={handleNext}>Next</button>
                                        </div>
                                    </>
                                )}

                                {step === 3 && (
                                    <>
                                        <h2 className='text-primary text-center'>Address</h2>
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
                                        <div className="d-flex justify-content-end">
                                            <button className="btn btn-secondary mx-2" type="button" onClick={handlePrevious}>Previous</button>
                                            <button className="btn btn-primary mx-2" type="button" onClick={handleNext}>Next</button>
                                        </div>
                                    </>
                                )}

                                {step === 4 && (
                                    <>
                                        <h2 className='text-primary text-center'>Physical Attributes</h2>
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
                                        <div className="d-flex justify-content-end">
                                            <button className="btn btn-secondary mx-2" type="button" onClick={handlePrevious}>Previous</button>
                                            <button className="btn btn-primary mx-2" type="button" onClick={handleNext}>Next</button>
                                        </div>
                                    </>
                                )}

                                {step === 5 && (
                                    <>
                                        <h2 className='text-primary text-center'>Family Info</h2>
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
                                        <div className="d-flex justify-content-end">
                                            <button className="btn btn-secondary mx-2" type="button" onClick={handlePrevious}>Previous</button>
                                            <button className="btn btn-primary mx-2" type="button" onClick={handleNext}>Next</button>
                                        </div>
                                    </>
                                )}

                                {step === 6 && (
                                    <>
                                        <h2 className='text-primary text-center'>Lifestyle</h2>
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
                                        <div className="d-flex justify-content-end">
                                            <button className="btn btn-secondary mx-2" type="button" onClick={handlePrevious}>Previous</button>
                                            <button className="btn btn-primary mx-2" type="button" onClick={handleNext}>Next</button>
                                        </div>
                                    </>
                                )}

                                {step === 7 && (
                                    <>
                                        <h2 className='text-primary text-center'>Education</h2>
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
                                        <div className="d-flex justify-content-end">
                                            <button type="button" className="btn btn-primary m-2" onClick={addEducation}>Add Education</button>
                                            <button className="btn btn-secondary m-2" type="button" onClick={handlePrevious}>Previous</button>
                                            <button className="btn btn-primary m-2" type="button" onClick={handleNext}>Next</button>
                                        </div>

                                    </>
                                )}


                                {step === 8 && (
                                    <>
                                        <h2 className='text-primary text-center'>Career</h2>
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
                                        <div className="d-flex justify-content-end">
                                            <button type="button" className="btn btn-primary m-2" onClick={addCareer}>Add Career</button>
                                            <button className="btn btn-secondary m-2" type="button" onClick={handlePrevious}>Previous</button>
                                            <button className="btn btn-primary m-2" type="button" onClick={handleNext}>Next</button>
                                        </div>

                                    </>
                                )}
                                {step === 9 && (
                                    <>
                                        <h2 className='text-primary text-center'>Gallery</h2>
                                        <div className="mb-3">
                                            <label htmlFor="gallery" className="form-label">Upload Images</label>
                                            <input
                                                type="file"
                                                className="form-control"
                                                id="gallery"
                                                name="gallery"
                                                multiple
                                                onChange={handleGalleryChange}
                                            />
                                        </div>
                                        <div className="d-flex justify-content-end">
                                            <button className="btn btn-secondary mx-2" type="button" onClick={handlePrevious}>Previous</button>
                                            <button className="btn btn-primary mx-2" type="button" onClick={handleNext}>Next</button>
                                        </div>
                                    </>
                                )}

                                {step === 10 && (
                                    <>
                                        <h2 className='text-primary text-center'>Partner Preferences</h2>
                                        <div className="row mb-3">
                                            <div className="col-md-6">
                                                <label htmlFor="heightMin" className="form-label">Minimum Height (cm)</label>
                                                <input
                                                    type="number"
                                                    className="form-control"
                                                    id="heightMin"
                                                    name="heightMin"
                                                    value={formData.partnerPreferences.heightMin}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                />
                                            </div>
                                            <div className="col-md-6">
                                                <label htmlFor="heightMax" className="form-label">Maximum Height (cm)</label>
                                                <input
                                                    type="number"
                                                    className="form-control"
                                                    id="heightMax"
                                                    name="heightMax"
                                                    value={formData.partnerPreferences.heightMax}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                />
                                            </div>
                                        </div>
                                        <div className="row mb-3">
                                            <div className="col-md-6">
                                                <label htmlFor="weightMin" className="form-label">Minimum Weight (Kg)</label>
                                                <input
                                                    type="number"
                                                    className="form-control"
                                                    id="weightMin"
                                                    name="weightMin"
                                                    value={formData.partnerPreferences.weightMin}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                />
                                            </div>
                                            <div className="col-md-6">
                                                <label htmlFor="weightMax" className="form-label">Maximum Weight (Kg)</label>
                                                <input
                                                    type="number"
                                                    className="form-control"
                                                    id="weightMax"
                                                    name="weightMax"
                                                    value={formData.partnerPreferences.weightMax}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                />
                                            </div>
                                        </div>
                                        <div className="row mb-3">
                                            <div className="col-md-6">
                                                <label htmlFor="maritalStatus" className="form-label">Marital Status</label>
                                                <select
                                                    className="form-control"
                                                    id="maritalStatus"
                                                    name="maritalStatus"
                                                    value={formData.partnerPreferences.maritalStatus}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                >
                                                    <option value="">Select Marital Status</option>
                                                    <option value="Single">Single</option>
                                                    <option value="Divorced">Divorced</option>
                                                    <option value="Widowed">Widowed</option>
                                                </select>
                                            </div>
                                            <div className="col-md-6">
                                                <label htmlFor="education" className="form-label">Minimum Education Qualification</label>
                                                <select
                                                    className="form-control"
                                                    id="education"
                                                    name="education"
                                                    value={formData.partnerPreferences.education}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                >
                                                    <option value="">Select Education Qualification</option>
                                                    <option value="High School">High School</option>
                                                    <option value="Bachelor's">Bachelor's</option>
                                                    <option value="Master's">Master's</option>
                                                    <option value="Ph.D.">Ph.D.</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div className="row mb-3">
                                            <div className="col-md-6">
                                                <label htmlFor="religion" className="form-label">Religion</label>
                                                <select
                                                    className="form-control"
                                                    id="religion"
                                                    name="religion"
                                                    value={formData.partnerPreferences.religion}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                >
                                                    <option value="">Select Religion</option>
                                                    {commonReligions.map(religion => (
                                                        <option key={religion} value={religion}>{religion}</option>
                                                    ))}
                                                </select>
                                            </div>
                                            <div className="col-md-6">
                                                <label htmlFor="language" className="form-label">Language</label>
                                                <select
                                                    className="form-control"
                                                    id="language"
                                                    name="language"
                                                    value={formData.partnerPreferences.language}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                >
                                                    <option value="">Select Language</option>
                                                    {commonLanguages.map(language => (
                                                        <option key={language} value={language}>{language}</option>
                                                    ))}
                                                </select>
                                            </div>
                                        </div>
                                        <div className="row mb-3">
                                            <div className="col-md-6">
                                                <label htmlFor="smokeAcceptable" className="form-label">Smoke Acceptable</label>
                                                <select
                                                    className="form-control"
                                                    id="smokeAcceptable"
                                                    name="smokeAcceptable"
                                                    value={formData.partnerPreferences.smokeAcceptable}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                >
                                                    <option value="">Select Option</option>
                                                    <option value="Yes">Yes</option>
                                                    <option value="No">No</option>
                                                </select>
                                            </div>
                                            <div className="col-md-6">
                                                <label htmlFor="drinkAcceptable" className="form-label">Drink Acceptable</label>
                                                <select
                                                    className="form-control"
                                                    id="drinkAcceptable"
                                                    name="drinkAcceptable"
                                                    value={formData.partnerPreferences.drinkAcceptable}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                >
                                                    <option value="">Select Option</option>
                                                    <option value="Yes">Yes</option>
                                                    <option value="No">No</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div className="row mb-3">
                                            <div className="col-md-6">
                                                <label htmlFor="state" className="form-label">State</label>
                                                <select
                                                    className="form-control"
                                                    id="state"
                                                    name="state"
                                                    value={formData.partnerPreferences.state}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                >
                                                    <option value="">Select State</option>
                                                    {statesOfIndia.map(state => (
                                                        <option key={state} value={state}>{state}</option>
                                                    ))}
                                                </select>
                                            </div>
                                            <div className="col-md-6">
                                                <label htmlFor="complexion" className="form-label">Complexion</label>
                                                <select
                                                    className="form-control"
                                                    id="complexion"
                                                    name="complexion"
                                                    value={formData.partnerPreferences.complexion}
                                                    onChange={handlePartnerPreferencesChange}
                                                    required
                                                >
                                                    <option value="">Select Complexion</option>
                                                    {complexions.map(complexion => (
                                                        <option key={complexion} value={complexion}>{complexion}</option>
                                                    ))}
                                                </select>
                                            </div>
                                        </div>
                                        <div className="d-flex justify-content-end">
                                            <button className="btn btn-secondary mx-2" type="button" onClick={handlePrevious}>Previous</button>
                                            <button className="btn btn-success mx-2" type="submit">Submit</button>
                                        </div>
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
