import React, { useState, memo, useEffect } from 'react';
import { useGameContext } from '../context/GameContext';
import { useUploadGameImage } from '../hooks/useUploadGameImage';

const GameRow = ({ game }) => {
  const { updateGameName, removeGame, editingGameId, setEditingGame } = useGameContext();
  const { handleUploadGameImage, gameImages } = useUploadGameImage();

  const [editedGameName, setEditedGameName] = useState(game.name);
  const [isEditing, setIsEditing] = useState(false);
  const [selectedFile, setSelectedFile] = useState(null);

  useEffect(() => {
    if (editingGameId === game.id) {
      setEditedGameName(game.name);
    }
  }, [editingGameId, game]);

  const handleFileChange = (e) => {
    setSelectedFile({ file: e.target.files[0], gameId: game.id });
  };

  const startEditing = () => {
    setEditingGame(game.id); 
    setIsEditing(true);
  };

  const cancelEditing = () => {
    setEditedGameName(game.name);
    setIsEditing(false);
    setEditingGame(null);  
  };

  const handleSave = async () => {
    if (editedGameName.trim()) {
      await updateGameName(game.id, editedGameName.trim());
      setIsEditing(false);
      setEditingGame(null); 
    }
  };

  const handleFileUpload = async () => {
    if (selectedFile && selectedFile.gameId === game.id) {
      await handleUploadGameImage(selectedFile.file, game.id);
    }
  };

  return (
    <tr>
      <td>
        {isEditing ? (
          <input
            type="text"
            value={editedGameName}
            onChange={(e) => setEditedGameName(e.target.value)}
            className="game-name-input"
          />
        ) : (
          <span>{game.name}</span>
        )}
      </td>
      <td>
        {gameImages[game.id] ? (
          <img src={URL.createObjectURL(gameImages[game.id])} alt="game" className="game-image" />
        ) : (
          <span>No Image</span>
        )}
      </td>
      <td>
        <input type="file" onChange={handleFileChange} accept="image/*" />
        <button onClick={handleFileUpload}>Upload</button>
      </td>
      <td>
        {isEditing ? (
          <>
            <button onClick={handleSave} className="saveBtn">Save</button>
            <button onClick={cancelEditing} className="cancelBtn">Cancel</button>
          </>
        ) : (
          <button onClick={startEditing} className="update-button">
            Edit
          </button>
        )}
        <button onClick={() => removeGame(game.id)} className="delete-button">Delete</button>
      </td>
    </tr>
  );
};

export default memo(GameRow);
