import { useState, useEffect } from 'react';

export const useUserRole = () => {
  const [userRole, setUserRole] = useState('');

  useEffect(() => {
    const fetchUserRoleFromLocalStorage = () => {
      const loggedInUser = localStorage.getItem('loggedInUser');
      if (loggedInUser) {
        const user = JSON.parse(loggedInUser);
        setUserRole(user.role);
      }
    };

    fetchUserRoleFromLocalStorage();
  }, []);

  return userRole;
};
