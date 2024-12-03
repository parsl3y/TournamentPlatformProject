import React, { useState, useEffect } from 'react';
import CreateGameComponent from './CreateGameComponent';
import GamesTable from './GamesTable'; 
import Loading from '../../../components/layouts/Loading'; 
import { fetchGames } from '../Services/gameService'; 
import 'react-toastify/dist/ReactToastify.css'; 

const GamesList = () => {
  
  const [games, setGames] = useState([]); 
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null); 
  const [isModalOpen, setIsModalOpen] = useState(false); 

  useEffect(() => {
    const loadGames = async () => {
      try {
        const gamesData = await fetchGames(); 
        setGames(gamesData); 
      } catch (error) {
        setError(error.message); 
        console.error('Error loading games:', error.message);
      } finally {
        setLoading(false); 
      }
    };

    loadGames(); 
  }, []);

  const handleOpenModal = () => {
    setIsModalOpen(true); 
  };

  const handleCloseModal = () => {
    setIsModalOpen(false); 
  };

  return (
    <div className="games-container">
      <button onClick={handleOpenModal} className="add-game-button">
        Add Games
      </button>

      {isModalOpen && (
        <div className="modal-overlay">
          <div className="modal-content">
            <button className="close-button" onClick={handleCloseModal}>
              ×
            </button>
            <CreateGameComponent games={games} setGames={setGames} />
          </div>
        </div>
      )}

      {loading ? (
        <Loading /> 
      ) : error ? (
        <div>Error: {error}</div>
      ) : (
        <GamesTable games={games} setGames={setGames} />
      )}
    </div>
  );
};

export default GamesList;
