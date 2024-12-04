import React, { useEffect, useState } from "react";
import { useUserRole } from '../../hooks/useUserRole'; 
import { useLogout } from '../../hooks/useLogOut'; 
import './NavBar.css';

const NavBar = () => {
    const [isVisible, setIsVisible] = useState(true);
    const [lastScrollY, setLastScrollY] = useState(0);
    const [isGamesMenuOpen, setIsGamesMenuOpen] = useState(false);

    const userRole = useUserRole();
    const { handleLogout } = useLogout();

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

                    {userRole === 'admin' && (
                        <div 
                            className="navButton" 
                            onClick={() => setIsGamesMenuOpen(!isGamesMenuOpen)} 
                            onMouseLeave={() => setIsGamesMenuOpen(false)} 
                        >
                            <span>Наповнення</span>
                            {isGamesMenuOpen && (
                                <div className="dropdownMenu">
                                    <button className="dropdownItem" onClick={() => window.location.href = "/games"}>Ігри</button>
                                    <button className="dropdownItem" onClick={() => window.location.href = "/countries"}>Країни</button>
                                    <button className="dropdownItem" onClick={() => window.location.href = "/teams"}>Команда</button>
                                    <button className="dropdownItem" onClick={() => window.location.href = "/players"}>Гравець</button>
                                </div>
                            )}
                        </div>
                    )}
                </nav>
                <div className="userActions">
                    <button className="iconButton" onClick={() => window.location.href = "/profile"}>👤</button>
                    <button className="iconButton" onClick={handleLogout}>🚪</button>
                </div>
            </header>
            <div className="bodyContent">
            </div>
        </>
    );
};

export default NavBar;
