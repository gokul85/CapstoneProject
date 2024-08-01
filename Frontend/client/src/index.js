import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { GoogleOAuthProvider } from '@react-oauth/google';
import { ToastContainer } from "react-toastify"
import 'react-toastify/dist/ReactToastify.css';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <>
    <GoogleOAuthProvider clientId="990387466122-j0s4eu2mehplp0ns17rgkgmbjss5rp5s.apps.googleusercontent.com">
      <App />
      <ToastContainer />
    </GoogleOAuthProvider>

  </>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
