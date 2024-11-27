import React, { useState } from "react";
import Login from "./Login";
import Register from "./Register";
import "./LoginForm.css";


const Authorization = ({ onLogin, onRegister, error }) => {
    const [isLoginView, setIsLoginView] = useState(true);

    const toggleView = () => {
        setIsLoginView(!isLoginView);
    };

    return (
        <div className="authWrapper">
            {isLoginView ? (
                <Login onLogin={onLogin} error={error} />
            ) : (
                <Register onRegister={onRegister} error={error} />
            )}
            <div className="linkContainer">
                <p>
                    {isLoginView ? "Немає акаунту?" : "Вже маєте акаунт?"}{" "}
                    <a onClick={toggleView} className="linkText">
                        {isLoginView ? "Зареєструватися" : "Увійти"}
                    </a>
                </p>
            </div>
        </div>
    );
};

export default Authorization;
