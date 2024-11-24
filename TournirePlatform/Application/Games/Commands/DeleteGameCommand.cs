using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Games.Exceptions;
using Domain.Countries;
using Domain.Game;
using MediatR;
using MediatR.Wrappers;

namespace Application.Games.Commands;

public class DeleteGameCommand : IRequest<Result<Game, GameException>>
{
    public required Guid GameId { get; set; }
}

public class DeleteGameCommandHandler(IGameQueries _gameQueries, IGameRepositories _gameRepositories) : IRequestHandler<DeleteGameCommand, Result<Game, GameException>>
{
    public async Task<Result<Game, GameException>> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var gameId = new GameId(request.GameId);
        
        var existingGame = await _gameQueries.GetById(gameId, cancellationToken);
        
        return await existingGame.Match<Task<Result<Game, GameException>>>(
            async g => await DeleteEntity(g, cancellationToken),
            () => Task.FromResult<Result<Game, GameException>>(new GameNotFoundException(gameId)));
    }
    
    public async Task<Result<Game, GameException>> DeleteEntity(Game game, CancellationToken cancellationToken)
    {
        try
        {
            return await _gameRepositories.Delete(game, cancellationToken);
        }
        catch(Exception e)
        {
            return new GameUknownException(game.Id, e);
        }
    }
}