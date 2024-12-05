import { useState } from 'react';
import { updateTeam } from '../Services/teamService';
import { toast } from 'react-toastify';

export const useUpdateTeam = () => {
  const [editedTeam, setEditedTeam] = useState({});
  const [isEditing, setIsEditing] = useState(null);

  const startEditing = (teamId, team) => {
    setIsEditing(teamId);
    setEditedTeam({ ...team });
  };

  const cancelEditing = () => {
    setIsEditing(null);
    setEditedTeam({});
  };

  const handleUpdateTeam = async (teamId, updatedTeam, teams, setTeams) => {
    try {
      const response = await updateTeam(teamId, updatedTeam);
      const updatedTeams = teams.map((team) =>
        team.id === teamId ? { ...team, ...updatedTeam } : team
      );
      setTeams(updatedTeams);
      toast.success('Team updated successfully!');
      cancelEditing();
    } catch (error) {
      toast.error('Failed to update team: ' + error.message);
    }
  };

  return {
    editedTeam,
    setEditedTeam,
    isEditing,
    startEditing,
    cancelEditing,
    handleUpdateTeam,
  };
};
