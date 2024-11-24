using Domain.Countries;
using Domain.Faculties;

namespace Application.Common.Interfaces.Repositories;

public interface IGameImageRepository
{
    Task<IReadOnlyList<GameImage>> GetAll(CancellationToken cancellationToken);
    Task<GameImage> Add(GameImage sneakerImage, CancellationToken cancellationToken);
    Task<GameImage> GetById(GameImageId id, CancellationToken cancellationToken);
    Task<IEnumerable<GameImage>> GetByGameId(GameId gameId, CancellationToken cancellationToken);
    Task<bool> Delete(GameImageId id, CancellationToken cancellationToken);
    Task<GameImage> Update(GameImage gameImage, CancellationToken cancellationToken);
}