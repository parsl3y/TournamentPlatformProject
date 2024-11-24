using Domain.Matches;
using Domain.Teams;
using Domain.TeamsMatchs;

namespace Test.Data;

public static class TeamMatchData
{
    public static TeamMatch MainTeamMatch(TeamMatchId teamMatchId, TeamId teamId, MatchId matchId)
        => TeamMatch.New(teamMatchId, teamId, matchId);
    
}   