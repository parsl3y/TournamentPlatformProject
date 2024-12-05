import React, { useState } from 'react';
import { createGame } from '../Services/gameService'; 
import { toast } from 'react-toastify'; 

const CreateGameComponent = ({ games, setGames }) => {
  const [newGameName, setNewGameName] = useState(''); 
  const [isCreating, setIsCreating] = useState(false);

  const handleCreateGame = async () => {
    if (!newGameName.trim()) {
      toast.error('Game name cannot be empty.');
      return;
    }

    try {
      setIsCreating(true);
      const newGame = await createGame(newGameName.trim());

      setGames((prevGames) => [...prevGames, newGame]);

      toast.success('Game created successfully!');
      setNewGameName('');
    } catch (error) {
      console.error('Error creating game:', error);
      toast.error('Error creating game: ' + error.message);
    } finally {
      setIsCreating(false);
    }
  };

  return (
    <div className="create-game-form">
      <h2>Create New Game</h2>
      <input
        type="text"
        value={newGameName}
        onChange={(e) => setNewGameName(e.target.value)} 
        placeholder="Enter game name"
        className="inputField"
        disabled={isCreating}
      />
      <button onClick={handleCreateGame} className="button" disabled={isCreating}>
        {isCreating ? 'Creating...' : 'Create Game'}
      </button>
    </div>
  );
};

export default CreateGameComponent;
