using Domain.Players;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IPlayerQueries
{
    Task<IReadOnlyList<Player>> GetAll(CancellationToken cancellationToken);
    Task<Option<Player>> GetById(PlayerId id, CancellationToken cancellationToken);
    Task<Option<Player>> GetByNickname(string nickName, CancellationToken cancellationToken);
}