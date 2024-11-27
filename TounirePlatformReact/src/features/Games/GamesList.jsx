import React, { useEffect, useState } from 'react';
import './GameList.css';
import { useCreateGame } from '../../hooks/useCreateGame';
import { useDeleteGame } from '../../hooks/useDeleteGame';
import { useUpdateGame } from '../../hooks/useUpdateGame';
import { useLogout } from '../../hooks/useLogOut'; 
import { useUploadGameImage } from '../../hooks/useUploadGameImage';
import NavBar from "../../Components/layout/NavBar";

const GamesList = () => {
  const [games, setGames] = useState([]);
  const [loading, setLoading] = useState(true);
  const [userRole, setUserRole] = useState('');
  const [newGameName, setNewGameName] = useState('');

  const { handleCreateGame, gameCreated } = useCreateGame();
  const { handleDeleteGame } = useDeleteGame();
  const { handleUpdateGame, setEditedGameName, editedGameName, isEditing, startEditing, cancelEditing } = useUpdateGame();
  const { handleUploadGameImage, gameImages, setGameImages } = useUploadGameImage();

  const { handleLogout } = useLogout();

  useEffect(() => {
    const fetchGames = async () => {
      try {
        const response = await fetch('http://localhost:5030/Games/GamesList');
        const data = await response.json();
        setGames(data);
      } catch (error) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    const fetchUserRole = () => {
      const loggedInUser = JSON.parse(localStorage.getItem('loggedInUser'));
      setUserRole(loggedInUser?.role || '');
    };

    fetchGames();
    fetchUserRole();
  }, []);

  const fetchGameImages = async (gameId) => {
    if (gameImages[gameId]) return;
    try {
      const response = await fetch(`http://localhost:5030/file/GameGet/${gameId}`);
      const images = await response.json();
      setGameImages((prev) => ({ ...prev, [gameId]: images }));
    } catch (error) {
      console.error('Error fetching game images:', error);
      alert('Error fetching game images');
    }
  };

  return (
    <>
      <NavBar onLogout={handleLogout} />
      <div className="games-container">
        <h1>Games List</h1>

        {userRole === 'admin' && (
          <div className="create-game-form">
            <h2>Create New Game</h2>
            <input
              type="text"
              value={newGameName}
              onChange={(e) => setNewGameName(e.target.value)}
              placeholder="Enter game name"
              className="inputField"
            />
            <button onClick={() => handleCreateGame(newGameName, games, setGames)} className="button">
              Create Game
            </button>
            {gameCreated && <p className="success-message">Game created successfully!</p>}
          </div>
        )}

        {loading ? (
          <div>Loading...</div>
        ) : (
          <table className="games-table">
            <thead>
              <tr>
                <th>Game Name</th>
                <th>Image</th>
                {userRole === 'admin' && <th>Upload Photo</th>}
                {userRole === 'admin' && <th>Action</th>}
              </tr>
            </thead>
            <tbody>
              {games.map((game) => (
                <tr key={game.id}>
                  <td>
                    {isEditing === game.id ? (
                      <input
                        type="text"
                        value={editedGameName}
                        onChange={(e) => setEditedGameName(e.target.value)}
                        className="inputField"
                      />
                    ) : (
                      game.name
                    )}
                  </td>
                  <td>
                    {gameImages[game.id] ? (
                      <img src={gameImages[game.id][0]?.s3Path} alt="Game" className="game-image" />
                    ) : (
                      <button onClick={() => fetchGameImages(game.id)} className="view-button">
                        Load Image
                      </button>
                    )}
                  </td>
                  {userRole === 'admin' && (
                    <>
                      <td>
                        <input
                          type="file"
                          onChange={(e) => handleUploadGameImage(e.target.files[0], game.id)}
                        />
                      </td>
                      <td>
                        {isEditing === game.id ? (
                          <>
                            <button
                              onClick={() => handleUpdateGame(game.id, editedGameName, games, setGames)}
                              className="view-button"
                            >
                              Save
                            </button>
                            <button onClick={cancelEditing} className="delete-button">
                              Cancel
                            </button>
                          </>
                        ) : (
                          <>
                            <button
                              onClick={() => startEditing(game.id, game.name)}
                              className="update-button"
                            >
                              Update
                            </button>
                            <button
                              onClick={() => handleDeleteGame(game.id, games, setGames)}
                              className="delete-button"
                            >
                              Delete
                            </button>
                          </>
                        )}
                      </td>
                    </>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </>
  );
};

export default GamesList;
