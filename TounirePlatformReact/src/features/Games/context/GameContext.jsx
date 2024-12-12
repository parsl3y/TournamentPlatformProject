import React, { createContext, useContext, useReducer, useEffect } from 'react';
import { fetchGames, createGame, updateGame, deleteGame } from '../Services/gameService';
import { toast } from 'react-toastify';

const GameContext = createContext();

export const useGameContext = () => {
  return useContext(GameContext);
};

const initialState = {
  games: [],
  loading: true,
  error: null,
  currentPage: 1,
  hasMoreGames: true,
  isModalOpen: false,
  editingGameId: null,
};

const gameReducer = (state, action) => {
  switch (action.type) {
    case 'FETCH_GAMES_SUCCESS':
      return {
        ...state,
        games: action.payload.games,
        loading: false,
        hasMoreGames: action.payload.hasMoreGames,
      };
    case 'FETCH_GAMES_FAILURE':
      return {
        ...state,
        loading: false,
        error: action.payload,
      };
    case 'ADD_GAME':
      return {
        ...state,
        games: [...state.games, action.payload],
      };
    case 'UPDATE_GAME':
      return {
        ...state,
        games: state.games.map((game) =>
          game.id === action.payload.id ? action.payload : game
        ),
      };
    case 'DELETE_GAME':
      return {
        ...state,
        games: state.games.filter((game) => game.id !== action.payload),
      };
    case 'SET_PAGE':
      return {
        ...state,
        currentPage: action.payload,
      };
    case 'TOGGLE_MODAL':
      return {
        ...state,
        isModalOpen: !state.isModalOpen,
      };
    case 'SET_EDITING_GAME':
      return {
        ...state,
        editingGameId: action.payload,
      };
    default:
      return state;
  }
};

export const GameProvider = ({ children }) => {
  const [state, dispatch] = useReducer(gameReducer, initialState);

  const fetchAndSetGames = async (page) => {
    try {
      const gamesData = await fetchGames(page);
      dispatch({
        type: 'FETCH_GAMES_SUCCESS',
        payload: {
          games: gamesData,
          hasMoreGames: gamesData.length === 5,
        },
      });
    } catch (error) {
      dispatch({ type: 'FETCH_GAMES_FAILURE', payload: error.message });
      toast.error('Error reloading games: ' + error.message);
    }
  };
  
  useEffect(() => {
    const loadGames = async () => {
      try {
        const gamesData = await fetchGames(state.currentPage);
        dispatch({
          type: 'FETCH_GAMES_SUCCESS',
          payload: {
            games: gamesData,
            hasMoreGames: gamesData.length === 5,
          },
        });
      } catch (error) {
        dispatch({ type: 'FETCH_GAMES_FAILURE', payload: error.message });
        toast.error('Error loading games: ' + error.message);
      }
    };

    loadGames();
  }, [state.currentPage]);

  const addGame = async (gameName) => {
    if (!gameName.trim()) {
      toast.error('Game name cannot be empty!');
      return;
    }
  
    const gameExists = state.games.some((game) => game.name === gameName.trim());
  
    if (gameExists) {
      toast.error('Game with this name already exists!');
      return;
    }
  
    try {
      await createGame(gameName.trim());
      toast.success('Game added successfully!');
      
      await fetchAndSetGames(state.currentPage);
    } catch (error) {
      toast.error('Error adding game: ' + error.message);
    }
  };
  

  const updateGameName = async (gameId, updatedGameName) => {
    if (!updatedGameName.trim()) {
      toast.error('Game name cannot be empty!');
      return;
    }
  
    const gameExists = state.games.some(
      (game) => game.name === updatedGameName.trim() && game.id !== gameId
    );
  
    if (gameExists) {
      toast.error('Game with this name already exists!');
      const game = state.games.find((game) => game.id === gameId);
      dispatch({ type: 'SET_EDITING_GAME_NAME', payload: game.name });
      return;
    }
  
    try {
      const updatedGame = await updateGame(gameId, updatedGameName.trim());
      dispatch({ type: 'UPDATE_GAME', payload: updatedGame });
      dispatch({ type: 'SET_EDITING_GAME', payload: null });
      toast.success('Game updated successfully!');
    } catch (error) {
      toast.error('Error updating game: ' + error.message);
      const game = state.games.find((game) => game.id === gameId);
      dispatch({ type: 'SET_EDITING_GAME_NAME', payload: game.name });
    }
  };
  
  

  const removeGame = async (gameId) => {
    try {
      await deleteGame(gameId);
      toast.success('Game deleted successfully!');
  
      await fetchAndSetGames(state.currentPage);
    } catch (error) {
      toast.error('Error deleting game: ' + error.message);
    }
  };
  
  const setPage = (pageNumber) => {
    dispatch({ type: 'SET_PAGE', payload: pageNumber });
  };

  const toggleModal = () => {
    dispatch({ type: 'TOGGLE_MODAL' });
  };

  const setEditingGame = (gameId) => {
    dispatch({ type: 'SET_EDITING_GAME', payload: gameId });
  };

  const value = {
    games: state.games,
    loading: state.loading,
    error: state.error,
    currentPage: state.currentPage,
    hasMoreGames: state.hasMoreGames,
    addGame,
    updateGameName,
    removeGame,
    setPage,
    isModalOpen: state.isModalOpen,
    toggleModal,
    editingGameId: state.editingGameId,
    setEditingGame,
  };

  return <GameContext.Provider value={value}>{children}</GameContext.Provider>;
};
