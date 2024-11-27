import React from "react";
import './UserProfile.css';

const UserProfile = ({ user, onLogout }) => {
  return (
    <div className="user-profile">
      <div className="profile-header">
        <img
          src={user.photoUrl || 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fstatic.vecteezy.com%2Fsystem%2Fresources%2Fpreviews%2F005%2F005%2F788%2Foriginal%2Fuser-icon-in-trendy-flat-style-isolated-on-grey-background-user-symbol-for-your-web-site-design-logo-app-ui-illustration-eps10-free-vector.jpg&f=1&nofb=1&ipt=431062aeda3df54eab4f4dcf521219cc1f33fcd494b7f343cc01568419fe2c3f&ipo=images'} 
          alt="User Avatar"
          className="profile-avatar"
        />
        <div className="profile-info">
          <p>Логін: {user.username}</p>
          <p>Роль: {user.role === "admin" ? "Адмін" : "Користувач"}</p>
        </div>
      </div>
    </div>
  );
};

export default UserProfile;
