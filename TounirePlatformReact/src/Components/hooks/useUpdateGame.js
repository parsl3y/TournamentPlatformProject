import { useState } from 'react';

export const useUpdateGame = () => {
  const [editedGameName, setEditedGameName] = useState('');
  const [isEditing, setIsEditing] = useState(null);  

  const handleUpdateGame = async (gameId, updatedGameName, games, setGames) => {
    if (!updatedGameName.trim()) {
      alert('Please provide a game name.');
      return;
    }

    try {

      setGames(games.map(game => game.id === gameId ? { ...game, name: updatedGameName } : game));  
      setIsEditing(null); 
      alert('Game updated successfully!');
    } catch (error) {
      console.error('Error updating game:', error.message);
      alert('Error updating game: ' + error.message);
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
