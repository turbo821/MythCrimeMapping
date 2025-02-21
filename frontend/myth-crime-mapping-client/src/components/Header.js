import React from "react";
import { Link } from "react-router-dom";
import "./Header.css";

const Header = () => {
  const token = localStorage.getItem("token");

  const handleLogout = () => {
    localStorage.removeItem("token");
  };

  return (
    <header className="header">
      <nav className="header-nav">
        <div className="nav-links">
          <p className="header-title">MythCrimeMap</p>
          <Link to="/" className="header-link">Карта</Link>
          <Link to="/crime-types" className="header-link">Виды преступлений</Link>
          <Link to="/wanted-persons" className="header-link">Преступники</Link>
          <Link to="/about" className="header-link">О приложении</Link>
        </div>
        {token ? (
          <div className="auth-links">
          <Link to="/account" className="header-link">Личный кабинет</Link>
          <Link onClick={handleLogout} to="/login" className="header-link">Выход</Link>
          </div>
        ) : (
          <div className="auth-links">
          <Link to="/login" className="header-link">Вход</Link>
          <Link to="/signup" className="header-link">Регистрация</Link>
        </div>
        )
      }

      </nav>
    </header>
  );
};

export default Header;
