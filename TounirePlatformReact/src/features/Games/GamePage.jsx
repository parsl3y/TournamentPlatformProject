import React from 'react';
import GameContainer from './components/GameContainer';  
import { GameProvider } from './context/GameContext'; // Import the GameProvider
import 'react-toastify/dist/ReactToastify.css';
import '../../Toast.css'; 

const GamePage = () => {
  return (
    <GameProvider>  
      <GameContainer />  
    </GameProvider>
  );
};

export default GamePage;
