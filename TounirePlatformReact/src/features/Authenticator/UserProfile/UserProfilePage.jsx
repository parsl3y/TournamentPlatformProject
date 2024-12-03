import React from "react";
import UserProfile from "./UserProfile";
import './UserProfilePage.css';
import { useLogout } from '../../../hooks/useLogOut'; 

const UserProfilePage = () => {
    const user = JSON.parse(localStorage.getItem("loggedInUser"));
    const { handleLogout } = useLogout();


    return (
        <div className="userProfilePage">
            <h1 className="H1">Профіль Користувача</h1>
            <main className="mainContent">
                {user ? (
                    <UserProfile user={user} onLogout={handleLogout} />
                ) : (
                    <p>Вам потрібно увійти, щоб побачити профіль.</p>
                )}
            </main>
        </div>
    );
};

export default UserProfilePage;
