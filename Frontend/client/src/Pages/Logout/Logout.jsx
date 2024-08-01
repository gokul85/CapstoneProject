import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from "react-toastify";

const Logout = () => {
    const navigate = useNavigate();
    useEffect(() => {
        localStorage.removeItem("token");
        toast.success("Successfully Logged Out!")
        navigate("/login");
    })
}
export default Logout;