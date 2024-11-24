using Domain.Players;
using Domain.Teams;
using Domain.TeamsMatchs;

namespace Api.Dtos;

public class TeamMatchCreateDto
{
    public Guid TeamId { get; set; }
    public Guid MatchId { get; set; }

    public TeamMatchCreateDto(Guid teamId, Guid matchId)
    {
        TeamId = teamId;
        MatchId = matchId;
    }

    public  static TeamMatchCreateDto FromDomainModel(TeamMatch teamMatch)
    {
        return new TeamMatchCreateDto(
            teamMatch.TeamId.Value,
            teamMatch.MatchId.Value
        );
    }
}
public class TeamMatchDto
{
    public Guid TeamId { get; set; }
    public string Team { get; set; }
    public int Score { get; set; }
    public bool? IsWinner { get; set; }
    public List<PlayerInTeamDto> Players { get; set; }

    public TeamMatchDto(string team, int score, bool? isWinner, List<PlayerInTeamDto> players)
    {
        Team = team;
        Score = score;
        IsWinner = null;
        Players = players;
    }

    public static TeamMatchDto FromDomainModel(TeamMatch teamMatch)
        =>
            new(
                teamMatch.Team.Name,
                teamMatch.Score,
                teamMatch.IsWinner,
                teamMatch.Team.PlayerTeams.Select(PlayerInTeamDto.FromDomainModel).ToList()
            );
}

public class TeamMatchDeleteDto
{
    public Guid TeamId { get; set; }

    public static TeamMatchDeleteDto FromDomainModel(TeamMatch teamMatch)
    {
        return new TeamMatchDeleteDto
        {
            TeamId = teamMatch.TeamId.Value
        };
    }
}

