import React, { useState } from "react";
import './LoginForm.css';

const Register = ({ onRegister, error }) => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [passwordStrength, setPasswordStrength] = useState(""); 
    const [validationError, setValidationError] = useState("");

    const handlePasswordChange = (e) => {
        const newPassword = e.target.value;
        setPassword(newPassword);
        checkPasswordStrength(newPassword); 
    };

    const checkPasswordStrength = (password) => {
        if (/^[a-zA-Z]+$/.test(password)) {
            setPasswordStrength("Bad");
        } else if (/^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$/.test(password)) {
            setPasswordStrength("Normal");
        } else if (/^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).+$/.test(password)) {
            setPasswordStrength("Good");
        } else {
            setPasswordStrength("");
        }
    };

    const handleRegisterClick = () => {
        if (!username || !password) {
            setValidationError("Пароль не може бути пустим та Ім'я не може бути пустим");
            return;
        }
        setValidationError("");
        onRegister(username, password);
        setUsername("");
        setPassword("");
        setPasswordStrength(""); 
    };

    return (
        <div className="loginForm">
            <h2>Зареєструватися</h2>
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
                onChange={handlePasswordChange}
                className="inputField"
            />
            {password && (
                <p
                    className="passwordStrength"
                    style={{
                        color: passwordStrength === "Bad" ? "red" :
                               passwordStrength === "Normal" ? "yellow" :
                               passwordStrength === "Good" ? "green" : "black"
                    }}
                >
                    {passwordStrength}
                </p>
            )}
            <button onClick={handleRegisterClick} className="button">
                Зареєструватися
            </button>
            {validationError && <p className="errorMessage">{validationError}</p>}
            {error && <p className="errorMessage">{error}</p>}
        </div>
    );
};

export default Register;
