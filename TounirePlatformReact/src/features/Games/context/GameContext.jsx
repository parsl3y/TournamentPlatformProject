import React, { createContext, useContext, useReducer, useEffect } from 'react';
import { fetchGames, createGame, updateGame, deleteGame } from '../Services/gameService';
import { toast } from 'react-toastify';

const GameContext = createContext();

export const useGameContext = () => useContext(GameContext);

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
      return { ...state, loading: false, error: action.payload };
    case 'ADD_GAME':
      return { ...state, games: [...state.games, action.payload] };
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
      return { ...state, currentPage: action.payload };
    case 'TOGGLE_MODAL':
      return { ...state, isModalOpen: !state.isModalOpen };
    case 'SET_EDITING_GAME':
      return { ...state, editingGameId: action.payload };
    default:
      return state;
  }
};

const fetchGamesData = async (page, dispatch) => {
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
    toast.error('Error loading games: ' + error.message);
  }
};

export const GameProvider = ({ children }) => {
  const [state, dispatch] = useReducer(gameReducer, initialState);

  useEffect(() => {
    fetchGamesData(state.currentPage, dispatch);
  }, [state.currentPage]);

  const isGameNameDuplicate = (gameName, gameId = null) => {
    return state.games.some(
      (game) => game.name === gameName.trim() && game.id !== gameId
    );
  };

  const addGame = async (gameName) => {
    if (!gameName.trim()) {
      toast.error('Game name cannot be empty!');
      return;
    }
    if (isGameNameDuplicate(gameName)) {
      toast.error('Game with this name already exists!');
      return;
    }
    try {
      await createGame(gameName.trim());
      toast.success('Game added successfully!');
      await fetchGamesData(state.currentPage, dispatch);
    } catch (error) {
      toast.error('Error adding game: ' + error.message);
    }
  };

  const updateGameName = async (gameId, updatedGameName) => {
    if (!updatedGameName.trim()) {
      toast.error('Game name cannot be empty!');
      return;
    }

    if (isGameNameDuplicate(updatedGameName, gameId)) {
      toast.error('Game with this name already exists!');
      return;
    }

    try {
      const updatedGame = await updateGame(gameId, updatedGameName.trim());
      dispatch({ type: 'UPDATE_GAME', payload: updatedGame });
      dispatch({ type: 'SET_EDITING_GAME', payload: null });
      toast.success('Game updated successfully!');
    } catch (error) {
      toast.error('Error updating game: ' + error.message);
    }
  };

  const removeGame = async (gameId) => {
    try {
      await deleteGame(gameId);
      toast.success('Game deleted successfully!');
      await fetchGamesData(state.currentPage, dispatch);
    } catch (error) {
      toast.error('Error deleting game: ' + error.message);
    }
  };

  const value = {
    ...state,
    addGame,
    updateGameName,
    removeGame,
    setPage: (pageNumber) => dispatch({ type: 'SET_PAGE', payload: pageNumber }),
    toggleModal: () => dispatch({ type: 'TOGGLE_MODAL' }),
    setEditingGame: (gameId) => dispatch({ type: 'SET_EDITING_GAME', payload: gameId }),
  };

  return <GameContext.Provider value={value}>{children}</GameContext.Provider>;
};
