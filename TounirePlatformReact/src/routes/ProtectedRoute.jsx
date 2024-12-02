import React from 'react';
import { Navigate } from 'react-router-dom';

const ProtectedRoute = ({ element }) => {
  const isAuthenticated = localStorage.getItem('loggedInUser'); 

  if (!isAuthenticated) {
    return <Navigate to="/auth" />;
  }

  return element; 
};

export default ProtectedRoute;
