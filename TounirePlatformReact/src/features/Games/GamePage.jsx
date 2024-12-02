import React from 'react';
import GamesList from './components/GamesList';  
import NavBar from '../../Components/layout/NavBar';  
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../../Toast.css'; 

const GamePage = () => {
  return (
    <>
      <NavBar />  

      <GamesList />  
      
      <ToastContainer />  
    </>
  );
};

export default GamePage;
