import { useState } from 'react';
import { updateCountry } from '../Services/countryService';
import { toast } from 'react-toastify';

export const useUpdateCountry = () => {
  const [editedCountryName, setEditedCountryName] = useState('');
  const [isEditing, setIsEditing] = useState(null);

  const handleUpdateCountry = async (countryId, updatedCountryName, countries, setCountries) => {
    if (!updatedCountryName.trim()) {
      toast.error('Country name cannot be empty!');
      return;
    }

    try {
      const updatedCountry = await updateCountry(countryId, updatedCountryName.trim());

      setCountries(
        countries.map(country =>
          country.id === countryId ? { ...country, name: updatedCountry.name } : country
        )
      );
      cancelEditing();

      toast.success('Country updated successfully!');
    } catch (error) {
      toast.error('Error updating country: ' + error.message);
    }
  };

  const startEditing = (countryId, countryName) => {
    setEditedCountryName(countryName);
    setIsEditing(countryId);
  };

  const cancelEditing = () => {
    setEditedCountryName('');
    setIsEditing(null);
  };

  return {
    handleUpdateCountry,
    setEditedCountryName,
    editedCountryName,
    isEditing,
    startEditing,
    cancelEditing,
  };
};
