using Domain.Players;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IPlayerImageRepository
    {
        Task<IReadOnlyList<PlayerImage>> GetAll(CancellationToken cancellationToken);
        Task<PlayerImage> Add(PlayerImage playerImage, CancellationToken cancellationToken);
        Task<PlayerImage> GetById(PlayerImageId id, CancellationToken cancellationToken);
        Task<IEnumerable<PlayerImage>> GetByPlayerId(PlayerId playerId, CancellationToken cancellationToken);
        Task<bool> ExistsByPlayerId(PlayerId playerId, CancellationToken cancellationToken);
        Task<bool> Delete(PlayerImageId id, CancellationToken cancellationToken);
        Task<PlayerImage> Update(PlayerImage playerImage, CancellationToken cancellationToken);
    }
}