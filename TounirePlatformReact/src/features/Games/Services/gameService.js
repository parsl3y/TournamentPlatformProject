import { HttpClient } from '../../../utils/http/HttpClient';

const httpClient = new HttpClient({ baseURL: 'http://localhost:5030' });

export const fetchGames = async (pageNumber = 1, pageSize = 5) => {
  try {
    const data = await httpClient.get(`/Games/GamesList?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    return data;
  } catch (error) {
    throw new Error('Error fetching games: ' + error.message); 
  }
};

export const createGame = async (newGameName) => {
  try {
    const newGame = { name: newGameName };
    const response = await httpClient.post('/Games/CreateGame', newGame);
    return response;
  } catch (error) {
    throw new Error('Error creating game: ' + error.message); 
  }
};

export const deleteGame = async (gameId) => {
  try {
    await httpClient.delete(`/Games/DeleteGame/${gameId}`);
  } catch (error) {
    throw new Error('Error deleting game: ' + error.message); 
  }
};

export const updateGame = async (gameId, updatedGameName) => {
  try {
    const updatedGame = { id: gameId, name: updatedGameName };
    const response = await httpClient.put('/Games/UpdateGame', updatedGame);
    return response;
  } catch (error) {
    throw new Error('Error updating game: ' + error.message); 
  }
};

export const fetchGameImages = async (gameId) => {
  try {
    const images = await httpClient.get(`/file/GameGet/${gameId}`);
    return images;
  } catch (error) {
    throw new Error('Error fetching game images: ' + error.message); 
  }
};

export const uploadGameImage = async (file, gameId) => {
  try {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('gameId', gameId);
    const response = await httpClient.post('/file/uploadGame', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
    return response;
  } catch (error) {
    throw new Error('Error uploading file: ' + error.message); 
  }
};
