import React from "react";
import { Routes, Route } from "react-router-dom";
import AuthPage from "../Authenticator/AuthPage"; 
import Profile from "../Authenticator/UserProfile/UserProfilePage";
import GamesList from "../Games/GamesList";
const AppRouter = () => {
  return (
    <Routes>
      <Route path="/auth" element={<AuthPage />} />
      <Route path="/profile" element={<Profile />} /> 
      <Route path="/games" element={<GamesList />} />
    </Routes>
  );
};

export default AppRouter;
