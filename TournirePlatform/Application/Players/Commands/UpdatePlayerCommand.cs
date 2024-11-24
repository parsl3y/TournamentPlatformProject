using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Players.Exceptions;
using Domain.Countries;
using Domain.Players;
using MediatR;

namespace Application.Players.Commands;

public class UpdatePlayerCommand : IRequest<Result<Player, PlayerException>>
{
    public required Guid PlayerId { get; init; }
    public required string NickName { get; init; }
    public required int Rating { get; init; }
    public required byte[] Photo { get; init; }
}

public class UpdatePlayerCommandHandler(IPlayerRepositories _playerRepositories, IPlayerQueries _playerQueries) : IRequestHandler<UpdatePlayerCommand, Result<Player, PlayerException>>
{
    public async Task<Result<Player, PlayerException>> Handle(UpdatePlayerCommand request,
        CancellationToken cancellationToken)
    {
        var playerId = new PlayerId(request.PlayerId);
        
        var existingPlayer = await _playerQueries.GetById(playerId, cancellationToken);

        return await existingPlayer.Match(
            async p => await UpdateEntity(p, request.NickName, request.Rating, request.Photo, cancellationToken),
            () => Task.FromResult<Result<Player,PlayerException>>(new PlayerNotFoundException(playerId)));
        
    }

    private async Task<Result<Player, PlayerException>> UpdateEntity(
        Player entity,
        string nickName,
        int rating,
        byte[] photo,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(nickName, rating, photo);
            return await _playerRepositories.Update(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new PlayerUknownException(entity.Id, e);
        }
    }
}
