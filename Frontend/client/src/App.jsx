import "bootstrap/dist/css/bootstrap.min.css";
import 'bootstrap/dist/js/bootstrap.bundle.min';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from "./Pages/Home/Home";
import Register from "./Pages/Auth/Register";
import Login from "./Pages/Auth/Login"
import "./App.css"
import AddProfileDetails from "./Pages/ProfileDetails/ProfileDetails"
import SearchPage from "./Pages/SearchPage/SearchPage";
import MemberDetails from "./Pages/MemberDetails/MemberDetails";
import Logout from "./Pages/Logout/Logout"
import Premium from "./Pages/Premium/Premium"
import Checkout from "./Pages/Checkout/Checkout"

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} />
        <Route path="/logout" element={<Logout />} />
        <Route path="/addprofile" element={<AddProfileDetails />} />
        <Route path="/search" element={<SearchPage />} />
        <Route path="/premium" element={<Premium />} />
        <Route path="/checkout" element={<Checkout />} />
        <Route path="/profile/:id" element={<MemberDetails />} />
      </Routes>
    </Router>
  );
}

export default App;

