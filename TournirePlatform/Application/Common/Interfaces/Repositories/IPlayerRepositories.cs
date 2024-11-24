using Domain.Players;

namespace Application.Common.Interfaces.Repositories;

public interface IPlayerRepositories
{
    Task<Player> Add (Player player, CancellationToken cancellationToken);
    Task<Player> Update  (Player player, CancellationToken cancellationToken);
    Task<Player> Delete (Player player, CancellationToken cancellationToken);
}