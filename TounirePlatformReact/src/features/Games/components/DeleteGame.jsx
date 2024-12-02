import React from 'react';
import { deleteGame } from '../Services/gameService';

const DeleteGame = ({ gameId, onDelete }) => {
  const handleDeleteGame = async () => {
    try {
      await deleteGame(gameId);

      onDelete((prevGames) => prevGames.filter((game) => game.id !== gameId)); 
    } catch (error) {
      alert('Error deleting game: ' + error.message);
    }
  };

  return (
    <button onClick={handleDeleteGame} className="delete-button">
      Delete Game
    </button>
  );
};

export default DeleteGame;
