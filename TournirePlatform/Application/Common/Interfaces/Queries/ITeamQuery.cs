using Domain.Players;
using Domain.Teams;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ITeamQuery
{
    Task<Option<Team>> GetById(TeamId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Team>> GetAll(CancellationToken cancellationToken);
    Task<Option<Team>> SearchByName(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Team>> GetAllIncluded(CancellationToken cancellationToken);
}