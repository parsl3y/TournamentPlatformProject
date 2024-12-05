import { HttpClient } from '../../../utils/http/HttpClient';
import { toast } from 'react-toastify';

const httpClient = new HttpClient({ baseURL: 'http://localhost:5030' });

export const fetchTeams = async () => {
  try {
    const data = await httpClient.get('/Teams/TeamList');
    return data;
  } catch (error) {
    toast.error('Error fetching teams: ' + error.message); 
    throw new Error('Error fetching teams: ' + error.message); 
  }
};

export const createTeam = async (teamData) => {
    try {
      const response = await httpClient.post('/Teams/CreateTeam', teamData);
      return response;
    } catch (error) {
      toast.error('Error creating team: ' + error.message);
      throw new Error('Error creating team: ' + error.message); 
    }
  };
  

export const deleteTeam = async (teamId) => {
  try {
    await httpClient.delete(`/Teams/DeleteTeam/${teamId}`);
  } catch (error) {
    toast.error('Error deleting game: ' + error.message);
    throw new Error('Error deleting team: ' + error.message); 
  }
};

export const updateTeam = async (teamId, updatedTeam) => {
  try {
    const response = await httpClient.put('/Teams/UpdateTeam', {
      id: teamId,
      name: updatedTeam.name,
      matchCount: updatedTeam.matchCount,
      winCount: updatedTeam.winCount,
    });
    return response;
  } catch (error) {
    toast.error('Error updating team: ' + error.message);
    throw new Error('Error updating team: ' + error.message);
  }
};

export const fetchTeamImages = async (teamId) => {
  try {
    const images = await httpClient.get(`/file/TeamGet/${teamId}`);
    return images;
  } catch (error) {
    toast.error('Error fetching team images: ' + error.message); 
    throw new Error('Error fetching team images: ' + error.message); 
  }
};

export const uploadTeamImage = async (file, teamId) => {
    const formData = new FormData();
    formData.append('image', file);
  
    const response = await fetch(`/api/teams/${teamId}/upload-image`, {
      method: 'POST',
      body: formData,
    });
  
    if (!response.ok) {
      throw new Error('Failed to upload image');
    }
  
    return await response.json();
  };
  