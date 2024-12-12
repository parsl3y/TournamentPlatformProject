import React, { useState } from 'react';
import { useGameContext } from '../context/GameContext';

const CreateGameComponent = () => {
  const { addGame } = useGameContext(); 
  const [newGameName, setNewGameName] = useState('');

  const handleCreateGame = () => {
    addGame(newGameName); 
    setNewGameName('');
  };

  return (
    <div className="create-game-form">
      <h2>Create New Game</h2>
      <input
        type="text"
        value={newGameName}
        onChange={(e) => setNewGameName(e.target.value)}
        placeholder="Enter game name"
        className="inputField"
      />
      <button onClick={handleCreateGame} className="button">
        Create Game
      </button>
    </div>
  );
};

export default CreateGameComponent;
