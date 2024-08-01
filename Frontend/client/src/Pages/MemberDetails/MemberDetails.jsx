import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import axiosInstance from "../../Utils/axiosInstance";
import { useNavigate, useParams } from 'react-router-dom';
import UserHeader from "../../Components/UserHeader";
import { jwtDecode } from 'jwt-decode';
import { Modal, Button } from 'react-bootstrap';

const MemberDetails = () => {
    const { id } = useParams();
    const [profile, setProfile] = useState(null);
    const [isPremium, setIsPremium] = useState(false);
    const [contactInfo, setContactInfo] = useState({ mobile: '*****', email: '*****' });
    const [showModal, setShowModal] = useState(false);
    const [dailyLimit, setDailyLimit] = useState(0);

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

        const fetchProfile = async () => {
            try {
                const response = await axiosInstance.get(`/search/viewprofile?profileid=${id}`);
                setProfile(response.data.result);
            } catch (error) {
                console.error("Error fetching profile data", error);
            }
        };

        fetchProfile();
    }, [id, navigate]);

    const handleViewContact = async () => {
        if (!isPremium) {
            navigate("/premium");
        } else {
            try {
                const response = await axiosInstance.get(`/premium/checkviewcontact?profileid=${id}`);
                if (response.data.result.remainingCount >= 0) {
                    setDailyLimit(response.data.result.remainingCount);
                    setShowModal(true);
                } else {
                    setContactInfo({
                        mobile: response.data.result.mobile,
                        email: response.data.result.email
                    });
                }
            } catch (error) {
                console.error("Error checking view contact", error);
            }
        }
    };

    const confirmViewContact = async () => {
        try {
            const response = await axiosInstance.post(`/premium/viewcontact`, { profileId: id });
            console.log(response.data);
            setContactInfo({
                mobile: response.data.result.mobile,
                email: response.data.result.email
            });
            setShowModal(false);
        } catch (error) {
            console.error("Error viewing contact", error);
        }
    };

    if (!profile) {
        return <div>Loading...</div>;
    }

    return (
        <>
            <div className="container-fluid">
                <UserHeader />
                <div className="px-3 px-lg-5">
                    <div className="mt-3 mb-4">
                        <div className="row align-items-center">
                            <div className="col-md-6">
                                <h1 className="h3">Member Details</h1>
                            </div>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col-lg-3">
                            <div className="card">
                                <div className="card-body text-center">
                                    <img
                                        src={profile.profileImage || 'default_image_url'}
                                        alt="avatar"
                                        className="rounded-circle mb-3"
                                        width="200px"
                                        height="200px"
                                        style={{ maxWidth: "100%" }}
                                    />
                                    <p></p>
                                    <div className="mb-2">
                                        <strong>Mobile:</strong> {contactInfo.mobile}
                                    </div>
                                    <div className="mb-3">
                                        <strong>Email:</strong> {contactInfo.email}
                                    </div>
                                    <button className="btn btn-info btn-sm" onClick={handleViewContact}>
                                        {isPremium ? 'View Contact' : 'Subscribe Premium'}
                                    </button>
                                </div>
                            </div>
                        </div>

                        <div className="col-lg-9">
                            {/* Introduction */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Introduction</h5>
                                </div>
                                <div className="card-body">
                                    <p>{profile.intro}</p>
                                </div>
                            </div>

                            {/* Basic Information */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Basic Information</h5>
                                </div>
                                <div className="card-body">
                                    <table className="table w-100">
                                        <tbody>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">First Name</th>
                                                <td className="col-6 col-md-auto">{profile.firstName}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Last Name</th>
                                                <td className="col-6 col-md-auto">{profile.lastName}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Gender</th>
                                                <td className="col-6 col-md-auto">{profile.gender}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Date Of Birth</th>
                                                <td className="col-6 col-md-auto">{new Date(profile.dob).getDate() + "-" + new Date(profile.dob).getMonth() + 1 + "-" + new Date(profile.dob).getFullYear()}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Marital Status</th>
                                                <td className="col-6 col-md-auto">{profile.maritalStatus}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">On Behalf</th>
                                                <td className="col-6 col-md-auto">{profile.onBehalf}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            {/* Education */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Education</h5>
                                </div>
                                <div className="card-body">
                                    <table className="table w-100">
                                        <thead>
                                            <tr>
                                                <th>Degree</th>
                                                <th>Specialization</th>
                                                <th>Start</th>
                                                <th>End</th>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {profile.educations.map((education, index) => (
                                                <tr key={index} className="d-flex d-md-table-row">
                                                    <td className="col-6 col-md-auto">{education.degree}</td>
                                                    <td className="col-6 col-md-auto">{education.specialization}</td>
                                                    <td className="col-6 col-md-auto">{education.startYear}</td>
                                                    <td className="col-6 col-md-auto">{education.endYear}</td>
                                                    <td className="col-6 col-md-auto">{education.status}</td>
                                                </tr>
                                            ))}
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            {/* Career */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Career</h5>
                                </div>
                                <div className="card-body">
                                    <table className="table w-100">
                                        <thead>
                                            <tr>
                                                <th>Designation</th>
                                                <th>Company</th>
                                                <th>Start</th>
                                                <th>End</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {profile.careers.map((career, index) => (
                                                <tr key={index} className="d-flex d-md-table-row">
                                                    <td className="col-6 col-md-auto">{career.jobTitle}</td>
                                                    <td className="col-6 col-md-auto">{career.company}</td>
                                                    <td className="col-6 col-md-auto">{career.startYear}</td>
                                                    <td className="col-6 col-md-auto">{career.endYear}</td>
                                                </tr>
                                            ))}
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            {/* Physical Attributes */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Physical Attributes</h5>
                                </div>
                                <div className="card-body">
                                    <table className="table w-100">
                                        <tbody>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Height</th>
                                                <td className="col-6 col-md-auto">{profile.height}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Weight</th>
                                                <td className="col-6 col-md-auto">{profile.weight}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Eye Color</th>
                                                <td className="col-6 col-md-auto">{profile.eyeColor}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Hair Color</th>
                                                <td className="col-6 col-md-auto">{profile.hairColor}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Complexion</th>
                                                <td className="col-6 col-md-auto">{profile.complexion}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Blood Group</th>
                                                <td className="col-6 col-md-auto">{profile.bloodGroup}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Disability</th>
                                                <td className="col-6 col-md-auto">{profile.disability}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            {/* Language */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Language</h5>
                                </div>
                                <div className="card-body">
                                    <table className="table w-100">
                                        <tbody>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Mother Tongue</th>
                                                <td className="col-6 col-md-auto">{profile.nativeLanguage}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            {/* Life Style */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Life Style</h5>
                                </div>
                                <div className="card-body">
                                    <table className="table w-100">
                                        <tbody>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Drink</th>
                                                <td className="col-6 col-md-auto">{profile.drink}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Smoke</th>
                                                <td className="col-6 col-md-auto">{profile.smoke}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Living With</th>
                                                <td className="col-6 col-md-auto">{profile.livingWith}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            {/* Religion Information */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Religion Information</h5>
                                </div>
                                <div className="card-body">
                                    <table className="table w-100">
                                        <tbody>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Religion</th>
                                                <td className="col-6 col-md-auto">{profile.religion}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Caste</th>
                                                <td className="col-6 col-md-auto">{profile.caste}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            {/* Location */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Location</h5>
                                </div>
                                <div className="card-body">
                                    <table className="table w-100">
                                        <tbody>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Country</th>
                                                <td className="col-6 col-md-auto">India</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">State</th>
                                                <td className="col-6 col-md-auto">{profile.state}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">City</th>
                                                <td className="col-6 col-md-auto">{profile.city}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            {/* Family Information */}
                            <div className="card mb-3">
                                <div className="card-header bg-dark text-white">
                                    <h5 className="mb-0">Family Information</h5>
                                </div>
                                <div className="card-body">
                                    <table className="table w-100">
                                        <tbody>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Father Status</th>
                                                <td className="col-6 col-md-auto">{profile.fatherStatus}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">Mother Status</th>
                                                <td className="col-6 col-md-auto">{profile.motherStatus}</td>
                                            </tr>
                                            <tr className="d-flex d-md-table-row">
                                                <th className="text-nowrap col-6 col-md-auto">No of Siblings</th>
                                                <td className="col-6 col-md-auto">{profile.noofsiblings}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            {/* Modal for confirming view contact */}
            <Modal show={showModal} onHide={() => setShowModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Confirm View Contact</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {dailyLimit > 0 ? (
                        <p>Daily limit remaining: {dailyLimit}. Are you sure you want to view the contact details?</p>
                    ) : (
                        <p>Daily limit reached. Please try again tomorrow.</p>
                    )}
                </Modal.Body>
                <Modal.Footer>
                    {dailyLimit > 0 && (
                        <Button variant="primary" onClick={confirmViewContact}>
                            Yes, view contact
                        </Button>
                    )}
                    <Button variant="secondary" onClick={() => setShowModal(false)}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

export default MemberDetails;
