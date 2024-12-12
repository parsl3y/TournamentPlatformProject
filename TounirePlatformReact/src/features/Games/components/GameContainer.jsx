import React, { useEffect } from 'react';
import CreateGame from './CreateGame';
import GamesTable from './GamesTable';
import Loading from '../../../components/layouts/Loading';
import { FaTimes } from 'react-icons/fa';
import { useGameContext } from '../context/GameContext';  

const GameContainer = () => {
  const {
    games,
    loading,
    error,
    currentPage,
    hasMoreGames,
    isModalOpen,
    toggleModal,
    setPage,
    addGame,
    removeGame,
  } = useGameContext(); 

  const handleOpenModal = () => {
    toggleModal(); 
  };

  const handleCloseModal = () => {
    toggleModal();  
  };

  const handleNextPage = () => {
    if (hasMoreGames) {
      setPage(currentPage + 1);  
    }
  };

  const handlePreviousPage = () => {
    setPage(Math.max(currentPage - 1, 1)); 
  };

  const moveLastGameToNextPage = (newGames) => {
    if (newGames.length > 5) {
      setPage(currentPage + 1);  
    }
  };

  return (
    <div className="games-container">
      <button onClick={handleOpenModal} className="add-game-button">
        Add Game
      </button>

      {isModalOpen && (
        <div className="modal-overlay">
          <div className="modal-content">
            <button className="close-button" onClick={handleCloseModal}>
              <FaTimes size={20} />
            </button>
            <CreateGame addGame={addGame} moveLastGameToNextPage={moveLastGameToNextPage} />
          </div>
        </div>
      )}

      {loading ? (
        <Loading />
      ) : error ? (
        <div className="error-message">Error: {error}</div>
      ) : (
        <GamesTable games={games} removeGame={removeGame} />
      )}

      <div className="pagination">
        <button onClick={handlePreviousPage} disabled={currentPage === 1}>
          Previous
        </button>
        <span>Page {currentPage}</span>
        <button onClick={handleNextPage} disabled={!hasMoreGames}>
          Next
        </button>
      </div>
    </div>
  );
};

export default GameContainer;
