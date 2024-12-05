import React, { useState } from 'react';
import { createCountry } from '../Services/countryService';
import { toast } from 'react-toastify';

const CreateCountryComponent = ({ countries, setCountries }) => {
  const [newCountryName, setNewCountryName] = useState('');

  const handleCreateCountry = async () => {
    try {
      const newCountry = await createCountry(newCountryName);
      setCountries([...countries, newCountry]);
      toast.success('Country created successfully!');
      setNewCountryName('');
    } catch (error) {
      toast.error('Error creating country: ' + error.message);
    }
  };

  return (
    <div className="create-country-form">
      <h2>Create New Country</h2>
      <input
        type="text"
        value={newCountryName}
        onChange={(e) => setNewCountryName(e.target.value)}
        placeholder="Enter country name"
        className="inputField"
      />
      <button onClick={handleCreateCountry} className="button">
        Create Country
      </button>
    </div>
  );
};

export default CreateCountryComponent;
