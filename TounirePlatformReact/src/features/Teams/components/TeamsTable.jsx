import React from 'react';
import PageTitle from '../../../components/layouts/PageTitle';
import TeamRow from './TeamRow';

const TeamsTable = ({ teams, setTeams }) => {
  return (
    <div className="teams-table-container">
      <PageTitle title="Team List" />

      <table className="teams-table">
        <thead>
          <tr>
            <th>Team Name</th>
            <th>Match Count</th>
            <th>Win Count</th>
            <th>Win Rate</th>
            <th>Image</th>
            <th>Image Upload</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {teams.map((team) => (
            <TeamRow key={team.id} team={team} teams={teams} setTeams={setTeams} />
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default TeamsTable;
