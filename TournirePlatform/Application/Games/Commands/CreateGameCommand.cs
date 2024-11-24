using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Games.Exceptions;
using Domain.Countries;
using Domain.Game;
using MediatR;

namespace Application.Games.Commands;

public class CreateGameCommand : IRequest<Result<Game, GameException>>
{
 public required string Name { get; init; }
}

public class CreateGameCommandHandler(
    IGameRepositories gameRepositories) : IRequestHandler<CreateGameCommand, Result<Game, GameException>>
{
    public async Task<Result<Game, GameException>> Handle(
        CreateGameCommand request,
        CancellationToken cancellationToken)
    {
        var existingGame = await gameRepositories.SearchByName(request.Name, cancellationToken);

        return await existingGame.Match(
            f => Task.FromResult<Result<Game, GameException>>(new GameAlreadExistsException(f.Id)),
            async () => await CreateEntity(request.Name, cancellationToken));
    }

    private async Task<Result<Game, GameException>> CreateEntity(string name, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new Game(GameId.New(), name);
            return await gameRepositories.Add(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new GameUknownException(GameId.Empty(), e);
        }
    }
}