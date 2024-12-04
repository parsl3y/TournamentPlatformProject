import React, { useState, memo } from 'react';
import DeleteGame from './DeleteGame';
import { useUpdateGame } from '../hooks/useUpdateGame';
import { useUploadGameImage } from '../hooks/useUploadGameImage';

const GameRow = ({ game, games, setGames }) => {
  const [selectedFile, setSelectedFile] = useState(null);
  const { handleUpdateGame, setEditedGameName, editedGameName, isEditing, startEditing, cancelEditing } = useUpdateGame();
  const { handleUploadGameImage, gameImages } = useUploadGameImage();

  const handleFileChange = (e) => {
    setSelectedFile({ file: e.target.files[0], gameId: game.id });
  };

  const handleFileUpload = async () => {
    if (selectedFile && selectedFile.gameId === game.id) {
      await handleUploadGameImage(selectedFile.file, game.id);
    }
  };

  return (
    <tr>
      <td>
        {isEditing === game.id ? (
          <input
            type="text"
            value={editedGameName}
            onChange={(e) => setEditedGameName(e.target.value)}
            placeholder="Enter new game name"
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
        {isEditing === game.id ? (
          <>
            <button
              onClick={() => handleUpdateGame(game.id, editedGameName, games, setGames)}
              className="saveBtn"
            >
              Save
            </button>
            <button onClick={cancelEditing} className="cancelBtn">
              Cancel
            </button>
          </>
        ) : (
          <>
            <button onClick={() => startEditing(game.id, game.name)} className="update-button">
              Edit
            </button>
            <DeleteGame gameId={game.id} onDelete={setGames} />
          </>
        )}
      </td>
    </tr>
  );
};

export default memo(GameRow);
