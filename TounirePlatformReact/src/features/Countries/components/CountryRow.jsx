import React, { useState, memo } from 'react';
import DeleteCountry from './DeleteCountry';
import { useUpdateCountry } from '../hooks/useUpdateCountry';
import { useUploadCountryImage } from '../hooks/useUploadCountryImage';

const CountryRow = ({ country, countries, setCountries }) => {
  const [selectedFile, setSelectedFile] = useState(null);
  const {
    handleUpdateCountry,
    setEditedCountryName,
    editedCountryName,
    isEditing,
    startEditing,
    cancelEditing,
  } = useUpdateCountry();
  const { handleUploadCountryImage, countryImages } = useUploadCountryImage();

  const handleFileChange = (e) => {
    setSelectedFile({ file: e.target.files[0], countryId: country.id });
  };

  const handleFileUpload = async () => {
    if (selectedFile && selectedFile.countryId === country.id) {
      await handleUploadCountryImage(selectedFile.file, country.id);
    }
  };

  return (
    <tr>
      <td>
        {isEditing === country.id ? (
          <input
            type="text"
            value={editedCountryName}
            onChange={(e) => setEditedCountryName(e.target.value)}
            placeholder="Enter new country name"
          />
        ) : (
          <span>{country.name}</span>
        )}
      </td>
      <td>
        {countryImages[country.id] ? (
          <img
            src={URL.createObjectURL(countryImages[country.id])}
            alt="country"
            className="country-image"
          />
        ) : (
          <span>No Image</span>
        )}
      </td>
      <td>
        <input type="file" onChange={handleFileChange} accept="image/*" />
        <button onClick={handleFileUpload}>Upload</button>
      </td>
      <td>
        {isEditing === country.id ? (
          <>
            <button
              onClick={() => handleUpdateCountry(country.id, editedCountryName, countries, setCountries)}
              className="saveBtn"
            >
              Save
            </button>
            <button onClick={cancelEditing} className="cancelBtn">
              Cancel
            </button>
          </>
        ) : (
          <>
            <button onClick={() => startEditing(country.id, country.name)} className="update-button">
              Edit
            </button>
            <DeleteCountry countryId={country.id} onDelete={setCountries} />
          </>
        )}
      </td>
    </tr>
  );
};

export default memo(CountryRow);
