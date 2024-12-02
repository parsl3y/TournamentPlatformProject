import React from "react";
import { Routes, Route } from "react-router-dom";
import AuthPage from "../features/Authenticator/AuthPage"; 
import Profile from "../features/Authenticator/UserProfile/UserProfilePage";
import GamePage from "../features/Games/GamePage";
import ProtectedRoute from './ProtectedRoute'; // Імпортуємо ProtectedRoute

const AppRouter = () => {
  return (
    <Routes>
      <Route path="/" element={<AuthPage />} />
      <Route path="/auth" element={<AuthPage />} />
      
      <Route 
        path="/profile" 
        element={<ProtectedRoute element={<Profile />} />} 
      />
      
      <Route path="/games" element={<GamePage />} />
    </Routes>
  );
};

export default AppRouter;
