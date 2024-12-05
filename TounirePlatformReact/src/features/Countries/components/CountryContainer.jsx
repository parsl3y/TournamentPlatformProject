import React, { useState, useEffect } from 'react';
import CreateCountry from './CreateCountry';
import CountriesTable from './CountryTable';
import Loading from '../../../components/layouts/Loading';
import { fetchCountries } from '../Services/countryService';
import 'react-toastify/dist/ReactToastify.css';
import { FaTimes } from 'react-icons/fa';

const CountryContainer = () => {
  const [countries, setCountries] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [hasMoreCountries, setHasMoreCountries] = useState(true);

  useEffect(() => {
    const loadCountries = async () => {
      try {
        const countriesData = await fetchCountries(currentPage);
        if (countriesData.length > 0) {
          setCountries(countriesData);
          setHasMoreCountries(countriesData.length === 5); 
        } else {
          setHasMoreCountries(false);
        }

        setTotalPages(Math.ceil(countriesData.totalCount / 5));
      } catch (error) {
        setError(error.message);
        console.error('Error loading countries:', error.message);
      } finally {
        setLoading(false);
      }
    };

    loadCountries();
  }, [currentPage]);

  const handleOpenModal = () => {
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  const handleNextPage = () => {
    if (hasMoreCountries) {
      setCurrentPage((prevPage) => prevPage + 1);
    }
  };

  const handlePreviousPage = () => {
    setCurrentPage((prevPage) => Math.max(prevPage - 1, 1));
  };

  const moveLastCountryToNextPage = (newCountries) => {
    const newCountryList = [...newCountries];
    if (newCountryList.length > 5) {
      const lastCountry = newCountryList.pop();
      setCountries(newCountryList);
      setCurrentPage((prevPage) => prevPage + 1);
    } else {
      setCountries(newCountryList);
    }
  };

  return (
    <div className="countries-container">
      <button onClick={handleOpenModal} className="add-country-button">
        Add Country
      </button>

      {isModalOpen && (
        <div className="modal-overlay">
          <div className="modal-content">
            <button className="close-button" onClick={handleCloseModal}>
              <FaTimes size={20} />
            </button>
            <CreateCountry
              countries={countries}
              setCountries={setCountries}
              moveLastCountryToNextPage={moveLastCountryToNextPage}
            />
          </div>
        </div>
      )}

      {loading ? (
        <Loading />
      ) : error ? (
        <div className="error-message">Error: {error}</div>
      ) : (
        <CountriesTable countries={countries} setCountries={setCountries} />
      )}

      <div className="pagination">
        <button onClick={handlePreviousPage} disabled={currentPage === 1}>
          Previous
        </button>
        <span>Page {currentPage}</span>
        <button onClick={handleNextPage} disabled={!hasMoreCountries}>
          Next
        </button>
      </div>
    </div>
  );
};

export default CountryContainer;
