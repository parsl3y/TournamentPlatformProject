import React, { useState, useEffect } from 'react';
import CreateGame from './CreateGame';
import GamesTable from './GamesTable'; 
import Loading from '../../../components/layouts/Loading'; 
import { fetchGames } from '../Services/gameService'; 
import 'react-toastify/dist/ReactToastify.css'; 
import { FaTimes } from 'react-icons/fa'; 

const GameContainer = () => {
  const [games, setGames] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);

  useEffect(() => {
    const loadGames = async () => {
      try {
        const gamesData = await fetchGames(currentPage); // Завантажуємо ігри для поточної сторінки
        setGames(gamesData);
        // Тут можна також оновити загальну кількість сторінок, якщо є така інформація
        // setTotalPages(totalPageCount); 
      } catch (error) {
        setError(error.message);
        console.error('Error loading games:', error.message);
      } finally {
        setLoading(false);
      }
    };

    loadGames();
  }, [currentPage]); // Завантажуємо нову сторінку при зміні currentPage

  const handleOpenModal = () => {
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  const handleNextPage = () => {
    setCurrentPage((prevPage) => prevPage + 1);
  };

  const handlePreviousPage = () => {
    setCurrentPage((prevPage) => Math.max(prevPage - 1, 1)); // Забезпечуємо мінімальну сторінку = 1
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
            <CreateGame games={games} setGames={setGames} />
          </div>
        </div>
      )}

      {loading ? (
        <Loading />
      ) : error ? (
        <div className="error-message">Error: {error}</div>
      ) : (
        <GamesTable games={games} setGames={setGames} />
      )}

      {/* Навігація по сторінках */}
      <div className="pagination">
        <button onClick={handlePreviousPage} disabled={currentPage === 1}>Previous</button>
        <span>Page {currentPage}</span>
        <button onClick={handleNextPage} disabled={currentPage === totalPages}>Next</button>
      </div>
    </div>
  );
};

export default GameContainer;
