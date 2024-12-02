import React, { useState, useEffect } from 'react';
import CreateGameComponent from './CreateGameComponent';
import GamesTable from './GamesTable'; 
import { fetchGames } from '../Services/gameService'; 
import 'react-toastify/dist/ReactToastify.css'; 

const GamesList = () => {
  const [games, setGames] = useState([]); 
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null); 

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

  return (
    <>
    <div className="games-container">

      <CreateGameComponent games={games} setGames={setGames} />

      {loading ? (
        <div>Loading...</div>
      ) : error ? (
        <div>Error: {error}</div>
      ) : (
        <GamesTable games={games} setGames={setGames} />
      )}
    </div>
    </>
  );
};

export default GamesList;
