import React from 'react';
import { Navigate } from 'react-router-dom';

export const ProtectedRoute = ({ element }) => {
  const isAuthenticated = localStorage.getItem('loggedInUser'); 

  if (!isAuthenticated) {
    return <Navigate to="/auth" />;
  }

  return element; 
};

export const ProtectedRouteIsLogin = ({ element }) => {
  const isAuthenticated = localStorage.getItem('loggedInUser'); 

  if (isAuthenticated) {
    return <Navigate to="/profile" />;
  }

  return element; 
};
