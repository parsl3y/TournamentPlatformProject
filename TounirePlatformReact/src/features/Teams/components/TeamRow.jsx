import React, { useState, memo } from 'react';
import DeleteTeam from './DeleteTeam'; 
import { useUpdateTeam } from '../hooks/useUpdateTeam';
import { useUploadTeamImage } from '../hooks/useUploadTeamImage'; 

const TeamRow = ({ team, teams, setTeams }) => {
  const [selectedFile, setSelectedFile] = useState(null);
  const { handleUpdateTeam, setEditedTeam, editedTeam, isEditing, startEditing, cancelEditing } = useUpdateTeam();
  const { handleUploadTeamImage, teamImages } = useUploadTeamImage();

  const handleFileChange = (e) => {
    setSelectedFile({ file: e.target.files[0], teamId: team.id });
  };

  const handleFileUpload = async () => {
    if (selectedFile && selectedFile.teamId === team.id) {
      await handleUploadTeamImage(selectedFile.file, team.id);
    }
  };

  return (
    <tr>
      <td>
        {isEditing === team.id ? (
          <input
            type="text"
            value={editedTeam.name}
            onChange={(e) => setEditedTeam({ ...editedTeam, name: e.target.value })}
            placeholder="Enter new team name"
          />
        ) : (
          <span>{team.name}</span>
        )}
      </td>
      <td>
        {isEditing === team.id ? (
          <input
            type="number"
            value={editedTeam.matchCount}
            onChange={(e) => setEditedTeam({ ...editedTeam, matchCount: e.target.value })}
            placeholder="Enter match count"
          />
        ) : (
          <span>{team.matchCount}</span>
        )}
      </td>
      <td>
        {isEditing === team.id ? (
          <input
            type="number"
            value={editedTeam.winCount}
            onChange={(e) => setEditedTeam({ ...editedTeam, winCount: e.target.value })}
            placeholder="Enter win count"
          />
        ) : (
          <span>{team.winCount}</span>
        )}
      </td>
      <td>
        {isEditing === team.id ? (
          <input
            type="number"
            value={editedTeam.winRate}
            onChange={(e) => setEditedTeam({ ...editedTeam, winRate: e.target.value })}
            placeholder="Enter win rate"
          />
        ) : (
          <span>{team.winRate}</span>
        )}
      </td>
      <td>
        {teamImages[team.id] ? (
          <img src={URL.createObjectURL(teamImages[team.id])} alt="team" className="team-image" />
        ) : (
          <span>No Image</span>
        )}
      </td>
      <td>
        <input type="file" onChange={handleFileChange} accept="image/*" />
        <button onClick={handleFileUpload}>Upload</button>
      </td>
      <td>
        {isEditing === team.id ? (
          <>
            <button
              onClick={() => handleUpdateTeam(team.id, editedTeam, teams, setTeams)}
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
            <button onClick={() => startEditing(team.id, team)} className="update-button">
              Edit
            </button>
            <DeleteTeam teamId={team.id} onDelete={setTeams} />
          </>
        )}
      </td>
    </tr>
  );
};

export default memo(TeamRow);
