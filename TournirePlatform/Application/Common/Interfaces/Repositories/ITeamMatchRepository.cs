using Domain.Matches;
using Domain.TeamsMatchs;
using Domain.Teams;

namespace Application.Common.Interfaces.Repositories;

public interface ITeamMatchRepository
{
    Task<Domain.TeamsMatchs.TeamMatch> Add(Domain.TeamsMatchs.TeamMatch teamMatch, CancellationToken cancellationToken);
    Task<bool> ChekIfTeamMatchExists(TeamId teamId, MatchId matchId, CancellationToken cancellationToken);

    Task<Domain.TeamsMatchs.TeamMatch> Delete(Domain.TeamsMatchs.TeamMatch teamMatch,
        CancellationToken cancellationToken);
    Task UpdateRange(IEnumerable<Domain.TeamsMatchs.TeamMatch> teamMatches, CancellationToken cancellationToken);
}