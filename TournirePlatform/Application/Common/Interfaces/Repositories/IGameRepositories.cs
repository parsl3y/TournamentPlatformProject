using Domain.Countries;
using Domain.Game;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IGameRepositories
{
    Task<Game> Add(Game game, CancellationToken cancellationToken);
    Task<Option<Game>> SearchByName(string name, CancellationToken cancellationToken);
    Task<Game> Update(Game faculty, CancellationToken cancellationToken);
    Task<Game> Delete(Game game, CancellationToken cancellationToken);
}