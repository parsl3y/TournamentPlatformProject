using Domain.Matches;

namespace Application.Common.Interfaces.Repositories;

public interface IMatchRepository
{
    Task<MatchGame> Add(MatchGame matchGame, CancellationToken cancellationToken);
    Task<MatchGame> Update(MatchGame matchGame, CancellationToken cancellationToken);
    Task<MatchGame> Delete(MatchGame match, CancellationToken cancellationToken);
}