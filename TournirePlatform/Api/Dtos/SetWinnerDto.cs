namespace Api.Dtos;

public class SetWinnerDto
{
    public Guid TeamId { get; set; }
    public Guid MatchId { get; set; }
}
public class WinnerResultDto
{
    public Guid MatchId { get; set; }
    public Guid WinnerTeamId { get; set; }

    public static WinnerResultDto FromDomainModel(Domain.TeamsMatchs.TeamMatch teamMatch)
    {
        return new WinnerResultDto
        {
            MatchId = teamMatch.MatchId.Value,
            WinnerTeamId = teamMatch.TeamId.Value
        };
    }
}