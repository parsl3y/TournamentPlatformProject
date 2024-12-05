import { useState } from 'react';
import { updateGame } from '../Services/gameService';
import { toast } from 'react-toastify';

export const useUpdateGame = () => {
  const [editedGameName, setEditedGameName] = useState('');
  const [isEditing, setIsEditing] = useState(null);

  const handleUpdateGame = async (gameId, updatedGameName, games, setGames) => {
    if (!updatedGameName.trim()) {
      toast.error('Please provide a valid game name.');
      return;
    }

    const currentGame = games.find((game) => game.id === gameId);
    if (currentGame && currentGame.name === updatedGameName.trim()) {
      toast.info('No changes detected.');
      cancelEditing();
      return;
    }

    try {
      const updatedGame = await updateGame(gameId, updatedGameName.trim());
      setGames(games.map(game => (game.id === gameId ? { ...game, name: updatedGame.name } : game)));
      cancelEditing();
      toast.success('Game updated successfully!');
    } catch (error) {
      console.error('Error updating game:', error);
      toast.error('Error updating game: ' + (error.message || 'Unknown error.'));
    }
  };

  const startEditing = (gameId, gameName) => {
    setEditedGameName(gameName);
    setIsEditing(gameId);
  };

  const cancelEditing = () => {
    setEditedGameName('');
    setIsEditing(null);
  };

  return {
    handleUpdateGame,
    setEditedGameName,
    editedGameName,
    isEditing,
    startEditing,
    cancelEditing,
  };
};
