using Domain.Game;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Countries;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class GameRepository(ApplicationDbContext context) : IGameRepositories, IGameQueries
{
    public async Task<IReadOnlyList<Game>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Games
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Game> Add(Game game, CancellationToken cancellationToken)
    {
        await context.Games.AddAsync(game, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return game;
    }

    public async Task<Option<Game>> SearchByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
        
        return entity == null ? Option.None<Game>() : Option.Some(entity);
    }
    public async Task<Option<Game>> GetById(GameId id, CancellationToken cancellationToken)
    {
        var entity = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Game>() : Option.Some(entity);
    }
    public async Task<Game> Update(Game game, CancellationToken cancellationToken)
    {
        context.Games.Update(game);

        await context.SaveChangesAsync(cancellationToken);

        return game;
    }
    public async Task<Game> Delete(Game game, CancellationToken cancellationToken)
    {
        context.Games.Remove(game);

        await context.SaveChangesAsync(cancellationToken);

        return game;
    }
}