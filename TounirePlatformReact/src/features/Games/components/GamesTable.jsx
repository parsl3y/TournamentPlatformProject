import React, { useState } from 'react';
import { useUploadGameImage } from '../hooks/useUploadGameImage'; 
import { useUpdateGame } from '../hooks/useUpdateGame'; 
import DeleteGame from './DeleteGame';  
import PageTitle from './PageTitle';
import '../GameList.css';

const GamesTable = ({ games, setGames }) => {
  const { handleUploadGameImage, gameImages } = useUploadGameImage(); 
  const { handleUpdateGame, setEditedGameName, editedGameName, isEditing, startEditing, cancelEditing } = useUpdateGame();
  const [selectedFile, setSelectedFile] = useState(null); 

  const handleFileChange = (e, gameId) => {
    setSelectedFile({ file: e.target.files[0], gameId });
  };

  const handleFileUpload = async (gameId) => {
    if (selectedFile && selectedFile.gameId === gameId) {
      await handleUploadGameImage(selectedFile.file, gameId); 
    }
  };

  return (
    <div className="games-table-container">
      <PageTitle title="Game List" />
      
      <table className="games-table">
        <thead>
          <tr>
            <th>Game Name</th>
            <th>Image</th>
            <th>Image Upload</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {games.map((game) => (
            <tr key={game.id}>
              <td>
                {isEditing === game.id ? (
                  <div>
                    <input
                      type="text"
                      value={editedGameName}
                      onChange={(e) => setEditedGameName(e.target.value)}
                      placeholder="Enter new game name"
                      className="inputField"
                    />
                  </div>
                ) : (
                  <div>
                    <span>{game.name}</span>
                  </div>
                )}
              </td>
              <td>
                {gameImages[game.id] ? (
                  <img
                    src={URL.createObjectURL(gameImages[game.id])}
                    alt="game"
                    className="game-image"
                  />
                ) : (
                  <span>No Image</span>
                )}
              </td>
              <td>
                <input
                  type="file"
                  onChange={(e) => handleFileChange(e, game.id)}
                  accept="image/*"
                  className="inputField"
                />
                <button 
                  onClick={() => handleFileUpload(game.id)} 
                  className="button"
                >
                  Upload
                </button>
              </td>
              <td>
                <div>
                  {isEditing === game.id ? (
                    <div>
                      <button 
                        onClick={() => handleUpdateGame(game.id, editedGameName, games, setGames)} 
                        className="update-button"
                      >
                        Save
                      </button>
                      <button onClick={cancelEditing} className="update-button">
                        Cancel
                      </button>
                    </div>
                  ) : (
                    <div>
                      <button 
                        onClick={() => startEditing(game.id, game.name)} 
                        className="update-button"
                      >
                        Edit
                      </button>
                      <DeleteGame 
                        gameId={game.id} 
                        onDelete={setGames} 
                        className="delete-button"
                      />
                    </div>
                  )}
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default GamesTable;
