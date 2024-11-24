using Domain.TeamsMatchs;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ITeamMatchQuery
{
    Task<Option<Domain.TeamsMatchs.TeamMatch>> GetById(TeamMatchId id, CancellationToken cancellationToken);
}