import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom"; 
import Authorization from "./Authentic/AuthForm";
import "./Authentic/LoginForm.css";
import { useLogout } from '../../hooks/useLogOut'; 

const AuthPage = () => {
    const [loggedInUser, setLoggedInUser] = useState(null);
    const [error, setError] = useState("");
    const [usersStorage, setUsersStorage] = useState([]);
    const navigate = useNavigate();

    const { handleLogout } = useLogout();
    useEffect(() => {
        const storedUser = JSON.parse(localStorage.getItem("loggedInUser"));
        const storedUsers = JSON.parse(localStorage.getItem("usersStorage")) || [];

        const adminUser = { username: "admin", password: "admin", role: "admin" };
        if (!storedUsers.some((user) => user.username === "admin")) {
            storedUsers.push(adminUser);
            localStorage.setItem("usersStorage", JSON.stringify(storedUsers));
        }

        setUsersStorage(storedUsers);
        if (storedUser) {
            setLoggedInUser(storedUser);
        }
    }, []);

    
    const handleLogin = (username, password) => {
        const user = usersStorage.find(
            (user) => user.username === username && user.password === password
        );
        if (user) {
            localStorage.setItem("loggedInUser", JSON.stringify(user));
            setLoggedInUser(user);
            setError("");
            navigate("/profile"); 
        } else {
            setError("Невірний логін або пароль");
        }
    };

    const handleRegister = (username, password) => {
        if (usersStorage.find((user) => user.username === username)) {
            setError("Користувач з таким ім'ям вже існує");
        } else {
            const newUser = { username, password, role: "user" };
            const updatedUsers = [...usersStorage, newUser];
            setUsersStorage(updatedUsers);
            localStorage.setItem("usersStorage", JSON.stringify(updatedUsers)); 
            localStorage.setItem("loggedInUser", JSON.stringify(newUser));
            setLoggedInUser(newUser);
            setError("");
            navigate("/profile");
        }
    };



    return (
        <div className="authContainer">
            <div className="imageContainer">
                <img
                    src="https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fimage.cnbcfm.com%2Fapi%2Fv1%2Fimage%2F105485043-1538560460468gettyimages-1042695306.jpeg%3Fv%3D1541788519&f=1&nofb=1&ipt=9e8ca3415f011f7e68a5a76bb62e057a83b15895614ed009859611b09e13b172&ipo=images"
                    alt="Background"
                    className="backgroundImage"
                />
            </div>
            <div className="contentContainer">
                {loggedInUser ? (
                    <>
                        <p>Вітаємо, {loggedInUser.username}!</p>
                        <button onClick={handleLogout} className="button">Вийти</button>
                    </>
                ) : (
                    <Authorization 
                        onLogin={handleLogin} 
                        onRegister={handleRegister} 
                        error={error} 
                    />
                )}
            </div>
            <footer>
            </footer>
        </div>
    );
};

export default AuthPage;
