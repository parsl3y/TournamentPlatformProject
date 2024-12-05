import React, { useState } from 'react';
import { toast } from 'react-toastify';
import { createTeam } from '../Services/teamService';

const CreateTeam = ({ teams, setTeams }) => {
  const [name, setName] = useState('');
  const [matchCount, setMatchCount] = useState(0);
  const [winCount, setWinCount] = useState(0);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!name.trim()) {
      toast.error('Please enter a team name');
      return;
    }

    const teamData = {
      name,
      matchCount,
      winCount,
    };

    setLoading(true);

    try {
      const response = await createTeam(teamData);
      toast.success(`Team ${response.name} created successfully!`);
      
      setTeams([...teams, response]);

      setName('');
      setMatchCount(0);
      setWinCount(0);
    } catch (error) {
      toast.error('Error creating team: ' + error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="create-team-container">
      <h2>Create New Team</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">Team Name</label>
          <input
            type="text"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="matchCount">Match Count</label>
          <input
            type="number"
            id="matchCount"
            value={matchCount}
            onChange={(e) => setMatchCount(Number(e.target.value))}
            required
          />
        </div>
        <div>
          <label htmlFor="winCount">Win Count</label>
          <input
            type="number"
            id="winCount"
            value={winCount}
            onChange={(e) => setWinCount(Number(e.target.value))}
            required
          />
        </div>
        <button type="submit" disabled={loading}>
          {loading ? 'Creating...' : 'Create Team'}
        </button>
      </form>
    </div>
  );
};

export default CreateTeam;
