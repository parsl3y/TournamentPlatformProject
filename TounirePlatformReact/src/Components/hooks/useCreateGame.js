import { useState } from 'react';
import { createGame } from '../Services/gameService';

export const useCreateGame = () => {
  const [gameCreated, setGameCreated] = useState(false);

  const handleCreateGame = async (newGameName, games, setGames) => {
    try {
      const newGame = await createGame(newGameName);
      setGames([...games, newGame]);
      setGameCreated(true);
    } catch (error) {
      alert('Error creating game: ' + error.message);
    }
  };

  return {
    handleCreateGame,
    gameCreated
  };
};
