using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Players.Exceptions;
using Domain.Players;
using MediatR;

namespace Application.Players.Commands;

public class DeletePlayerCommand : IRequest<Result<Player,PlayerException>>
{
    public required Guid PlayerId { get; set; }
}

public class DeletePlayerCommandHandler(IPlayerQueries _playerQueries, IPlayerRepositories _playerRepositories)
    : IRequestHandler<DeletePlayerCommand, Result<Player, PlayerException>>
{
    public async Task<Result<Player, PlayerException>> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var playerId = new PlayerId(request.PlayerId);
        
        var existingPlayer = await _playerQueries.GetById(playerId, cancellationToken);

        return await existingPlayer.Match<Task<Result<Player, PlayerException>>>(
            async p => await DeleteEntity(p, cancellationToken),
            () => Task.FromResult<Result<Player, PlayerException>>(new PlayerNotFoundException(playerId)));
    }

    public async Task<Result<Player, PlayerException>> DeleteEntity(Player player, CancellationToken cancellationToken)
    {
        try
        {
            return await _playerRepositories.Delete(player, cancellationToken);
        }
        catch(Exception e)
        {
            return new PlayerUknownException(player.Id, e);
        }
    }
}
