import React, { useState } from "react";
import './LoginForm.css';

const Login = ({ onLogin, error }) => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [validationError, setValidationError] = useState("");

    const handleLoginClick = () => {
        if (!username || !password) {
            setValidationError("Пароль не може бути пустим та Ім'я не може бути пустим");
            return;
        }
        setValidationError("");
        onLogin(username, password);
        setUsername("");
        setPassword("");
    };

    return (
        <div className="loginForm">
            <h2>Увійти</h2>
            <input
                type="text"
                placeholder="Логін"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="inputField"
            />
            <input
                type="password"
                placeholder="Пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="inputField"
            />
            <button onClick={handleLoginClick} className="button">
                Увійти
            </button>
            {validationError && <p className="errorMessage">{validationError}</p>}
            {error && <p className="errorMessage">{error}</p>}
        </div>
    );
};

export default Login;
