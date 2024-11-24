using System.Reflection;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Games.Exceptions;
using Domain.Countries;
using Domain.Game;
using MediatR;
using Optional;

namespace Application.Games.Commands;

public record UpdateGameCommand : IRequest<Result<Game,GameException>>
{
    public required Guid GameId { get; init; }
    public required string Name { get; init; }
}

public class UpdateGameCommandHandler(
    IGameRepositories gameRepositories, IGameQueries gameQueries) : IRequestHandler<UpdateGameCommand, Result<Game, GameException>>
{
    public async Task<Result<Game, GameException>> Handle(UpdateGameCommand request,
        CancellationToken cancellationToken)
    {
        var gameId = new GameId(request.GameId);
        var game = await gameQueries.GetById(gameId, cancellationToken);

        return await game.Match(
            async f =>
            {
                var existingFaculty = await CheckDuplicated(gameId, request.Name, cancellationToken);

                return await existingFaculty.Match(
                    ef => Task.FromResult<Result<Game, GameException>>(new GameAlreadExistsException(ef.Id)),
                    async () => await UpdateEntity(f, request.Name, cancellationToken));
            },
            () => Task.FromResult<Result<Game, GameException>>(new GameNotFoundException(gameId)));
    }

    private async Task<Result<Game, GameException>> UpdateEntity(
        Game game,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            game.UpdateDetails(name);

            return await gameRepositories.Update(game, cancellationToken);
        }
        catch (Exception exception)
        {
            return new GameUknownException(game.Id, exception);
        }
    }
private async Task<Option<Game>> CheckDuplicated(
    GameId gameId,
    string name,
    CancellationToken cancellationToken)
{
    var game = await gameRepositories.SearchByName(name, cancellationToken);

    return game.Match(
        g => g.Id == gameId ? Option.None<Game>() : Option.Some(g),
        Option.None<Game>);
}

}