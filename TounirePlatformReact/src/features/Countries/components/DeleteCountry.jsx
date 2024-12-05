import React from 'react';
import { deleteCountry } from '../Services/countryService';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const DeleteCountry = ({ countryId, onDelete }) => {
  const handleDeleteCountry = async () => {
    try {
      await deleteCountry(countryId);

      onDelete((prevCountries) =>
        prevCountries.filter((country) => country.id !== countryId)
      );

      toast.success('Country deleted successfully!');
    } catch (error) {
      toast.error('Error deleting country: ' + error.message);
    }
  };

  return (
    <button onClick={handleDeleteCountry} className="delete-button">
      Delete Country
    </button>
  );
};

export default DeleteCountry;
