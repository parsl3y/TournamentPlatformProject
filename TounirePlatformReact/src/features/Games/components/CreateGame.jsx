import React, { useState } from 'react';
import { createGame } from '../Services/gameService';
import { toast } from 'react-toastify';

const CreateGameComponent = ({ games, setGames, moveLastGameToNextPage }) => {
  const [newGameName, setNewGameName] = useState('');

  const handleCreateGame = async () => {
    try {
      const newGame = await createGame(newGameName);
      const updatedGames = [...games, newGame]; // додайте нову гру в список
      moveLastGameToNextPage(updatedGames); // Перемістіть на останню сторінку
      toast.success('Game created successfully!');
      setNewGameName('');
    } catch (error) {
      toast.error('Error creating game: ' + error.message);
    }
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
