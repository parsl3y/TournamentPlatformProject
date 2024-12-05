import React from 'react';
import PageTitle from '../../../components/layouts/PageTitle';
import GameRow from './GameRow';
import '../GameList.css';

const GamesTable = ({ games, setGames }) => {
  return (
    <div className="games-table-container">
      <PageTitle title="Game List" />

      <table className="games-table">
        <thead>
          <tr>
            <th>Game Name</th>
            <th>Image</th>
            <th>Image Upload</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {games.map((game) => (
            <GameRow key={game.id} game={game} games={games} setGames={setGames} />
          ))}
        </tbody>
      </table>
    </div>
  );d
};

export default GamesTable;
