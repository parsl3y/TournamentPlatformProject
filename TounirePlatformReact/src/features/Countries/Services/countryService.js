import { HttpClient } from '../../../utils/http/HttpClient';
import { toast } from 'react-toastify';

const httpClient = new HttpClient({ baseURL: 'http://localhost:5030' });

export const fetchCountries = async (pageNumber = 1, pageSize = 5) => {
  try {
    const data = await httpClient.get(`/Country/CountriesList?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    return data;
  } catch (error) {
    toast.error('Error fetching country: ' + error.message);
    throw new Error('Error fetching country: ' + error.message);
  }
};

export const createCountry = async (newCountryName) => {
  try {
    const newCountry = { name: newCountryName };
    const response = await httpClient.post('/Country/CreateCountry', newCountry);
    return response;
  } catch (error) {
    toast.error('Error creating country: ' + error.message);
    throw new Error('Error creating country: ' + error.message);
  }
};

export const deleteCountry = async (countryId) => {
  try {
    await httpClient.delete(`/Country/DeleteCoutry/${countryId}`);
  } catch (error) {
    toast.error('Error deleting country: ' + error.message);
    throw new Error('Error deleting country: ' + error.message);
  }
};

export const updateCountry = async (countryId, updatedCountryName) => {
  try {
    const updatedCountry = { id: countryId, name: updatedCountryName };
    const response = await httpClient.put('/Country/UpdateCountry', updatedCountry);
    return response;
  } catch (error) {
    toast.error('Error updating country: ' + error.message);
    throw new Error('Error updating country: ' + error.message);
  }
};

export const fetchCountryImages = async (countryId) => {
  try {
    const images = await httpClient.get(`/file/CountryGet/${countryId}`);
    return images;
  } catch (error) {
    toast.error('Error fetching country images: ' + error.message);
    throw new Error('Error fetching country images: ' + error.message);
  }
};

export const uploadCountryImage = async (file, countryId) => {
  try {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('countryId', countryId);
    const response = await httpClient.post('/file/uploadCountry', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
    return response;
  } catch (error) {
    toast.error('Error uploading file: ' + error.message);
    throw new Error('Error uploading file: ' + error.message);
  }
};
