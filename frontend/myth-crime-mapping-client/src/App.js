import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Layout from "./components/Layout";
import "./App.css";
import MapPage from "./pages/MapPage";
import WantedPersonsPage from "./pages/WantedPersonsPage";
import CrimeTypesListPage from "./pages/CrimeTypesListPage";
import AboutPage from "./pages/AboutPage";
import SignInPage from "./pages/SignInPage";
import SignUpPage from "./pages/SignUpPage";
import ProfilePage from "./pages/ProfilePage";

const App = () => {
  return (
    <Router>
        <Routes>
          <Route path="/" element={<Layout isMapPage={true}><MapPage /></Layout>} />
          <Route path="/crime-types" element={<Layout><CrimeTypesListPage /></Layout>} />
          <Route path="/wanted-persons" element={<Layout><WantedPersonsPage /></Layout>} />
          <Route path="/about" element={<Layout><AboutPage /></Layout>} />
          <Route path="/login" element={<Layout><SignInPage /></Layout>} />
          <Route path="/signup" element={<Layout><SignUpPage /></Layout>} />  
          <Route path="/profile/:id" element={<Layout><ProfilePage /></Layout>} />
        </Routes>
    </Router>
  );
};

export default App;