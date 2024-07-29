import React from 'react';

const HowItWorks = () => {
    return (
        <section className="py-5 bg-white">
            <div className="container">
                <div className="row">
                    <div className="col-lg-10 col-xl-8 col-xxl-6 mx-auto">
                        <div className="text-center mb-5">
                            <h2 className="fw-bolder mb-3 text-primary">How It Works</h2>
                            <p className="fw-normal fs-5 text-muted">
                                When you realize you want to spend the rest of your life with somebody, you want the rest of your life to start as soon as possible.
                            </p>
                        </div>
                    </div>
                </div>
                <div className="row g-3">
                    <div className="col-lg-4">
                        <div className="border p-3 mb-3 h-100">
                            <div className="row align-items-center">
                                <div className="col-7">
                                    <div className="text-primary fw-bold display-4">1</div>
                                    <div className="text-secondary fs-4 fw-bold">Sign up</div>
                                    <div className="fs-6 text-muted">
                                        Register for free & put up your Profile
                                    </div>
                                </div>
                                <div className="col-5 text-end">
                                    <img
                                        src="/Images/howitworks1.png"
                                        className="img-fluid"
                                        style={{ height: '80px' }}
                                        alt="Sign up"
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col-lg-4">
                        <div className="border p-3 mb-3 h-100">
                            <div className="row align-items-center">
                                <div className="col-7">
                                    <div className="text-primary fw-bold display-4">2</div>
                                    <div className="text-secondary fs-4 fw-bold">Connect</div>
                                    <div className="fs-6 text-muted">
                                        Select & Connect with Matches you like
                                    </div>
                                </div>
                                <div className="col-5 text-end">
                                    <img
                                        src="/Images/howitworks2.png"
                                        className="img-fluid"
                                        style={{ height: '80px' }}
                                        alt="Connect"
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col-lg-4">
                        <div className="border p-3 mb-3 h-100">
                            <div className="row align-items-center">
                                <div className="col-7">
                                    <div className="text-primary fw-bold display-4">3</div>
                                    <div className="text-secondary fs-4 fw-bold">Interact</div>
                                    <div className="fs-6 text-muted">
                                        Become a Premium Member & Start a Conversation
                                    </div>
                                </div>
                                <div className="col-5 text-end">
                                    <img
                                        src="/Images/howitworks3.png"
                                        className="img-fluid"
                                        style={{ height: '80px' }}
                                        alt="Interact"
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
};

export default HowItWorks;
