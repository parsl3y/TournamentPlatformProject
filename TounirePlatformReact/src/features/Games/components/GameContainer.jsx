import React from 'react';
import CreateGame from './CreateGame';
import GamesTable from './GamesTable';
import Loading from '../../../components/layouts/Loading';
import PaginationControls from './PaginationControls';
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
    toggleModalAction,
    setPageAction,
    addGame,
    removeGame,
  } = useGameContext(); 

  return (
    <div className="games-container">
      <button onClick={toggleModalAction} className="add-game-button">
        Add Game
      </button>

      {isModalOpen && (
        <div className="modal-overlay">
          <div className="modal-content">
            <button className="close-button" onClick={toggleModalAction}>
              <FaTimes size={20} />
            </button>
            <CreateGame addGame={addGame}  />
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

      <PaginationControls 
        currentPage={currentPage} 
        hasMoreGames={hasMoreGames} 
        setPageAction={setPageAction} 
      />
    </div>
  );
};

export default GameContainer;
