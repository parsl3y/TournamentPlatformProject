using Domain.TeamsMatchs;

public class AddScoreDto
{
    public Guid TeamId { get; set; }
    public Guid MatchId { get; set; }
}

public class TeamMatchScoreDto
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid MatchId { get; set; }
    public int Score { get; set; }
    public bool? IsWinner { get; set; }

    public static TeamMatchScoreDto FromDomainModel(TeamMatch teamMatch) => new()
    {
        Id = teamMatch.Id.Value,
        TeamId = teamMatch.TeamId.Value,
        MatchId = teamMatch.MatchId.Value,
        Score = teamMatch.Score,
        IsWinner = teamMatch.IsWinner
    };
}