import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import AuthPage from "../features/Authenticator/AuthPage"; 
import Profile from "../features/Authenticator/UserProfile/UserProfilePage";
import GamePage from "../features/Games/GamePage";
import ProtectedRoute from './ProtectedRoute'; 
import Layout from "../components/layouts/Layout"; 

const AppRouter = () => {
  return (
    <Routes>
      <Route path="/auth" element={<AuthPage />} />

      <Route path="/" element={<Layout />}>

        <Route index element={<AuthPage />} />

        <Route 
          path="profile" 
          element={<ProtectedRoute element={<Profile />} />} 
        />
        
      <Route 
          path="games" 
          element={<ProtectedRoute element={<GamePage />} />} 
        />

        </Route>

      

      <Route path="*" element={<Navigate to="/auth" replace />} />
    </Routes>
  );
};

export default AppRouter;
