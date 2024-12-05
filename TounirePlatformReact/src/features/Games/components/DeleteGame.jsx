import React from 'react';
import { deleteGame } from '../Services/gameService';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css'; 

const DeleteGame = ({ gameId, onDelete }) => {
  const handleDeleteGame = async () => {
    try {
      await deleteGame(gameId);

      onDelete((prevGames) => prevGames.filter((game) => game.id !== gameId)); 
    
      toast.success('Team deleted successfully!');
    } catch (error) {
      toast.error('Error deleting team: ' + error.message); 
    }
  };

  return (
    <button onClick={handleDeleteGame} className="delete-button">
      Delete Game
    </button>
  );
};

export default DeleteGame;
