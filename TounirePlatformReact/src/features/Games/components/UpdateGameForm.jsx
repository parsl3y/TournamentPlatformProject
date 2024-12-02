import React, { useState } from 'react';
import { useUpdateGame } from './useUpdateGame'; 

const UpdateGameForm = ({ game, games, setGames }) => {
  const {
    handleUpdateGame,
    setEditedGameName,
    editedGameName,
    isEditing,
    startEditing,
    cancelEditing
  } = useUpdateGame();

  const handleStartEditing = () => {
    startEditing(game.id, game.name);
  };

  const handleSave = () => {
    handleUpdateGame(game.id, editedGameName, games, setGames);
    cancelEditing(); 
  };

  return (
    <div>
      {isEditing === game.id ? (
        <div>
          <input
            type="text"
            value={editedGameName}
            onChange={(e) => setEditedGameName(e.target.value)}
            placeholder="Enter new game name"
          />
          <button onClick={handleSave}>Save</button>
          <button onClick={cancelEditing}>Cancel</button>
        </div>
      ) : (
        <div>
          <span>{game.name}</span>
          <button onClick={handleStartEditing}>Edit</button>
        </div>
      )}
    </div>
  );
};

export default UpdateGameForm;
