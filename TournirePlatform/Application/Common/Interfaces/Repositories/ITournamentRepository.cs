using Domain.Tournaments;

namespace Application.Common.Interfaces.Repositories;

public interface ITournamentRepository
{
    Task<Tournament> Add(Tournament tournament, CancellationToken cancellationToken);
    Task<Tournament> Update(Tournament tournament, CancellationToken cancellationToken);
    Task<Tournament> Delete(Tournament tournament, CancellationToken cancellationToken);
}