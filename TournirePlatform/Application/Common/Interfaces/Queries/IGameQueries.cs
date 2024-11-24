using Domain.Countries;
using Domain.Game;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IGameQueries
{
    Task<IReadOnlyList<Game>> GetAll(CancellationToken cancellationToken);
    Task<Option<Game>> GetById(GameId id, CancellationToken cancellationToken);

}