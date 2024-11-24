using Domain.Matches;
using Domain.Players;
using Domain.Teams;

namespace Domain.TeamsMatchs;

public class TeamMatch
{
    public TeamMatchId Id { get; private set; }
    public TeamId TeamId { get; private set; }
    public Team Team { get; set; }
    public MatchId MatchId { get; private set; }
    public MatchGame MatchGame { get; set; }
    public int Score { get; set; }
    public bool? IsWinner { get; set; }
    

    private TeamMatch(TeamMatchId id, TeamId teamId, MatchId matchId, int score, bool? isWinner)
    {
        Id = id;
        TeamId = teamId;
        MatchId = matchId;
        Score = score;
    }

    public static TeamMatch New(TeamMatchId id ,TeamId teamId, MatchId matchId)
        => new (id, teamId, matchId, 0, false);
}