using Domain.Tournaments;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ITournamentQueries
{
    Task<Option<Tournament>> GetById(TournamentId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Tournament>> GetAll( CancellationToken cancellationToken);
    Task<Option<Tournament>> SearchByName(string name, CancellationToken cancellationToken);
}