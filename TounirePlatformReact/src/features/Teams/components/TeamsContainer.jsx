import React, { useState, useEffect } from 'react';
import CreateTeam from './CreateTeam'; 
import TeamsTable from './TeamsTable'; 
import Loading from '../../../components/layouts/Loading'; 
import { fetchTeams } from '../Services/teamService'; 
import 'react-toastify/dist/ReactToastify.css'; 

const TeamContainer = () => {
  
  const [teams, setTeams] = useState([]); 
  const [loading, setLoading] = useState(true); 
  const [error, setError] = useState(null); 
  const [isModalOpen, setIsModalOpen] = useState(false); 

  useEffect(() => {
    const loadTeams = async () => {
      try {
        const teamsData = await fetchTeams(); 
        setTeams(teamsData); 
      } catch (error) {
        setError(error.message); 
        console.error('Error loading teams:', error.message);
      } finally {
        setLoading(false); 
      }
    };

    loadTeams(); 
  }, []);

  const handleOpenModal = () => {
    setIsModalOpen(true); 
  };

  const handleCloseModal = () => {
    setIsModalOpen(false); 
  };

  return (
    <div className="teams-container">
      <button onClick={handleOpenModal} className="add-team-button">
        Add Team
      </button>

      {isModalOpen && (
        <div className="modal-overlay">
          <div className="modal-content">
            <button className="close-button" onClick={handleCloseModal}>
              &times;
            </button>
            <CreateTeam teams={teams} setTeams={setTeams} />
          </div>
        </div>
      )}

      {loading ? (
        <Loading /> 
      ) : error ? (
        <div>Error: {error}</div> 
      ) : (
        <TeamsTable teams={teams} setTeams={setTeams} /> 
      )}
    </div>
  );
};

export default TeamContainer;
