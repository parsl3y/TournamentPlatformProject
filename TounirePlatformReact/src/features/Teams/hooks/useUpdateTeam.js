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
    if (!updatedTeam.name.trim()) {
      toast.error('Team name cannot be empty!');
      return;
    }

    if (!updatedTeam.matchCount || updatedTeam.matchCount <= 0) {
      toast.error('Match count cannot be empty or zero!');
      return;
    }

    if (updatedTeam.winCount > updatedTeam.matchCount) {
      toast.error('Wins cannot be greater than match count!');
      return;
    }

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
