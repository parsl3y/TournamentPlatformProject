import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import AuthPage from "../features/Authenticator/AuthPage";
import Profile from "../features/Authenticator/UserProfile/UserProfilePage";
import GamePage from "../features/Games/GamePage";
import { ProtectedRoute, ProtectedRouteIsLogin } from "./ProtectedRoute";
import Layout from "../components/layouts/Layout";
import TeamPage from "../features/Teams/TeamPage";

const AppRouter = () => {
  return (
    <Routes>
      <Route 
        path="/auth" 
        element={<ProtectedRouteIsLogin element={<AuthPage />} />} 
      />

      <Route path="/" element={<Layout />}>
        <Route index element={<Navigate to="/auth" replace />} />

        <Route 
          path="profile" 
          element={<ProtectedRoute element={<Profile />} />} 
        />

        <Route 
          path="games" 
          element={<ProtectedRoute element={<GamePage />} />} 
        />

        {/* Додано роут для команд */}
        <Route 
          path="teams" 
          element={<ProtectedRoute element={<TeamPage />} />} 
        />
      </Route>

      <Route path="*" element={<Navigate to="/auth" replace />} />
    </Routes>
  );
};

export default AppRouter;
