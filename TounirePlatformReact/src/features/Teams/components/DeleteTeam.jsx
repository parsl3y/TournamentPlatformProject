import React from 'react';
import { deleteTeam } from '../Services/teamService'; 
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css'; 

const DeleteTeam = ({ teamId, onDelete }) => {
  const handleDeleteTeam = async () => {
    try {
      await deleteTeam(teamId); 

      onDelete((prevTeams) => prevTeams.filter((team) => team.id !== teamId));
      
      toast.success('Team deleted successfully!');
    } catch (error) {
      toast.error('Error deleting team: ' + error.message); 
    }
  };

  return (
    <>
      <button onClick={handleDeleteTeam} className="delete-button">
        Delete Team
      </button>

    </>
  );
};

export default DeleteTeam;
