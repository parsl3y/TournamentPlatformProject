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

const actionHandlers = {
  FETCH_GAMES_SUCCESS: (state, action) => ({
    ...state,
    games: action.payload.games,
    loading: false,
    hasMoreGames: action.payload.hasMoreGames,
  }),
  FETCH_GAMES_FAILURE: (state, action) => ({
    ...state,
    loading: false,
    error: action.payload,
  }),
  ADD_GAME: (state, action) => ({
    ...state,
    games: [...state.games, action.payload],
  }),
  UPDATE_GAME: (state, action) => ({
    ...state,
    games: state.games.map((game) =>
      game.id === action.payload.id ? action.payload : game
    ),
  }),
  DELETE_GAME: (state, action) => ({
    ...state,
    games: state.games.filter((game) => game.id !== action.payload),
  }),
  SET_PAGE: (state, action) => ({
    ...state, 
    currentPage: action.payload,
  }),
  TOGGLE_MODAL: (state) => ({
    ...state,
    isModalOpen: !state.isModalOpen,
  }),
  SET_EDITING_GAME: (state, action) => ({
    ...state,
    editingGameId: action.payload,
  }),
};

const gameReducer = (state, action) => {
  const handler = actionHandlers[action.type];
  return handler ? handler(state, action) : state;
};

const fetchGamesDataAction = async (page, dispatch) => {
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
    fetchGamesDataAction(state.currentPage, dispatch);
  }, [state.currentPage]);

  const isGameNameDuplicate = (gameName, gameId = null) => {
    return state.games.some(
      (game) => game.name === gameName.trim() && game.id !== gameId
    );
  };

  const addGameAction = async (gameName) => {
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
      await fetchGamesDataAction(state.currentPage, dispatch);
    } catch (error) {
      toast.error('Error adding game: ' + error.message);
    }
  };

  const removeGameAction = async (gameId) => {
    try {
      await deleteGame(gameId);
      toast.success('Game deleted successfully!');
      await fetchGamesDataAction(state.currentPage, dispatch);
    } catch (error) {
      toast.error('Error deleting game: ' + error.message);
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

  const value = {
    ...state,
    addGameAction,
    updateGameName,
    removeGameAction,
    setPageAction: (pageNumber) => dispatch({ type: 'SET_PAGE', payload: pageNumber }),
    toggleModalAction: () => dispatch({ type: 'TOGGLE_MODAL' }),
    setEditingGameAction: (gameId) => dispatch({ type: 'SET_EDITING_GAME', payload: gameId }),
  };

  return <GameContext.Provider value={value}>{children}</GameContext.Provider>;
};
