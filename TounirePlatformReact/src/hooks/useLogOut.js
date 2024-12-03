import { useNavigate } from 'react-router-dom';

export const useLogout = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("loggedInUser");  
    navigate("/auth");  
    window.location.reload();
  };

  return { handleLogout };
};
