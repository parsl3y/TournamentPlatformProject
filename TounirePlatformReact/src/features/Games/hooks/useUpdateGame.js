import { useState } from 'react';
import { updateGame } from '../Services/gameService';
import { toast } from 'react-toastify';

export const useUpdateGame = () => {
  const [editedGameName, setEditedGameName] = useState('');
  const [isEditing, setIsEditing] = useState(null);  

  const handleUpdateGame = async (gameId, updatedGameName, games, setGames) => {
    if (!updatedGameName.trim()) {
      toast.error('Please provide a game name.');
      return;
    }

    try {
      const updatedGame = await updateGame(gameId, updatedGameName);

      setGames(games.map(game => game.id === gameId ? { ...game, name: updatedGame.name } : game));

      setIsEditing(null);
      setEditedGameName('');

      toast.success('Game updated successfully!');
    } catch (error) {
      console.error('Error updating game:', error.message);
      toast.error('Error updating game: ' + error.message);
    }
  };

  const startEditing = (gameId, gameName) => {
    setEditedGameName(gameName);
    setIsEditing(gameId);  
  };

  const cancelEditing = () => {
    setIsEditing(null); 
    setEditedGameName(''); 
  };

  return {
    handleUpdateGame,
    setEditedGameName,
    editedGameName,
    isEditing,
    startEditing,
    cancelEditing
  };
};
