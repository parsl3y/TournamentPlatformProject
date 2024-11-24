using Domain.Teams;

namespace Application.Common.Interfaces.Repositories;

public interface ITeamRepository
{
    Task<Team> Add(Team team, CancellationToken cancellationToken);
    Task<Team> Update(Team team, CancellationToken cancellationToken);
    Task<Team> Delete(Team team, CancellationToken cancellationToken);
}