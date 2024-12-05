import React from 'react';
import { deleteTeam } from '../Services/teamService'; 

const DeleteTeam = ({ teamId, onDelete }) => {
  const handleDeleteTeam = async () => {
    try {
      await deleteTeam(teamId); 

      onDelete((prevTeams) => prevTeams.filter((team) => team.id !== teamId));
    } catch (error) {
      alert('Error deleting team: ' + error.message); 
    }
  };

  return (
    <button onClick={handleDeleteTeam} className="delete-button">
      Delete Team
    </button>
  );
};

export default DeleteTeam;
