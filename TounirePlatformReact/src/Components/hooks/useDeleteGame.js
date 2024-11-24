import { deleteGame } from '../Services/gameService';

export const useDeleteGame = () => {
  const handleDeleteGame = async (gameId, games, setGames) => {
    try {
      await deleteGame(gameId);
      setGames(games.filter((game) => game.id !== gameId));
    } catch (error) {
      alert('Error deleting game: ' + error.message);
    }
  };

  return {
    handleDeleteGame
  };
};
