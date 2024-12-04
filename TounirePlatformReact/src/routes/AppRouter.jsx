import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import AuthPage from "../features/Authenticator/AuthPage";
import Profile from "../features/Authenticator/UserProfile/UserProfilePage";
import GamePage from "../features/Games/GamePage";
import { ProtectedRoute, ProtectedRouteIsLogin } from "./ProtectedRoute";
import Layout from "../components/layouts/Layout";

const AppRouter = () => {
  return (
    <Routes>
      {/* Сторінка авторизації */}
      <Route 
        path="/auth" 
        element={<ProtectedRouteIsLogin element={<AuthPage />} />} 
      />

      {/* Роутинг із Layout */}
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
      </Route>

      {/* Перенаправлення для всіх інших шляхів */}
      <Route path="*" element={<Navigate to="/auth" replace />} />
    </Routes>
  );
};

export default AppRouter;
