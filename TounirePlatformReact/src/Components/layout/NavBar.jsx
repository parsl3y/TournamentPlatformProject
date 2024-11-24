import React, { useEffect, useState } from "react";
import './NavBar.css';

const NavBar = ({ onLogout }) => {
    const [isVisible, setIsVisible] = useState(true);
    const [lastScrollY, setLastScrollY] = useState(0);

    useEffect(() => {
        const handleScroll = () => {
            const currentScrollY = window.scrollY;

            if (currentScrollY > lastScrollY && currentScrollY > 100) {
                setIsVisible(false); 
            } else {
                setIsVisible(true); 
            }

            setLastScrollY(currentScrollY);
        };

        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    }, [lastScrollY]);

    return (
        <>
            <header className={`header ${isVisible ? 'visible' : 'hidden'}`}>
                <div className="logo">
                    Tournament
                </div>
                <nav className="nav">
                    <button className="navButton">Матчі</button>
                    <button className="navButton">Турніри</button>
                    <button className="navButton">Команди</button>
                    <button className="navButton">Гравці</button>
                    <button className="navButton" onClick={() => window.location.href = "/games"}>Ігри</button>
                </nav>
                <div className="userActions">
                    <button className="iconButton" onClick={() => window.location.href = "/profile"}>👤</button>
                    <button className="iconButton" onClick={onLogout}>🚪</button>
                </div>
            </header>
            <div className="bodyContent">
            </div>
        </>
    );
};

export default NavBar;
