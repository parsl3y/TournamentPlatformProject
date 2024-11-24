using Domain.Countries;
using Domain.Players;
using Domain.TeamsMatchs;
using Domain.Teams;
using Domain.Tournaments;

namespace Domain.Matches;

public class MatchGame
{
    public MatchId Id { get; private set; }
    public string Name { get; private set; }
    public GameId GameId { get; private set; } 
    public Game.Game Game { get; private set; }
    public DateTime StartAt { get; private set; }

    public int MaxTeams { get; private set; }
    public TournamentId? TournamentId { get; set; }
    public Tournament? Tournament { get; private set; }
    public ICollection<TeamMatch> TeamMatches { get; private set; } = [];
    public int ScoreForGetWinner { get; private set; }
    public bool IsFinished { get; private set; }

    private MatchGame(MatchId id, string name, GameId gameId, DateTime startAt, int maxTeams, TournamentId? tournamentId, int scoreForGetWinner, bool isFinished)
    {
        Id = id;
        Name = name;
        GameId = gameId;
        StartAt = startAt;
        MaxTeams  = maxTeams;
        TournamentId = tournamentId;
        ScoreForGetWinner = scoreForGetWinner;
        IsFinished = isFinished;
    }
    
    public static MatchGame New(MatchId id, string name, GameId gameId, DateTime startAt, int maxTeams, int scoreForGetWinner,
        TournamentId? tournamentId)
        => new (id, name, gameId, startAt, maxTeams, tournamentId,scoreForGetWinner, false);
    
    public void MarkAsFinished()
    {
        IsFinished = true;
    }


 
}