using Domain.Matches;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IMatchQueries
{
    Task<Option<MatchGame>> GetById(MatchId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<MatchGame>> GetAll(CancellationToken cancellationToken);
    Task<Option<MatchGame>> GetMatchByName(string id, CancellationToken cancellationToken);

}