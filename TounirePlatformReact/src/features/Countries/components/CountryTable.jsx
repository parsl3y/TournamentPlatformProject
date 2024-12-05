import React from 'react';
import PageTitle from '../../../components/layouts/PageTitle';
import CountryRow from './CountryRow';
import '../CountryList.css';

const CountriesTable = ({ countries, setCountries }) => {
  return (
    <div className="countries-table-container">
      <PageTitle title="Country List" />

      <table className="countries-table">
        <thead>
          <tr>
            <th>Country Name</th>
            <th>Image</th>
            <th>Image Upload</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {countries.map((country) => (
            <CountryRow key={country.id} country={country} countries={countries} setCountries={setCountries} />
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default CountriesTable;
