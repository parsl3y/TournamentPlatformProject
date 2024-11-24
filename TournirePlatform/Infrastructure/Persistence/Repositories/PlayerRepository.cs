using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Game;
using Domain.Players;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class PlayerRepository(ApplicationDbContext context) : IPlayerQueries, IPlayerRepositories
{
    public async Task<Option<Player>> GetById(PlayerId id, CancellationToken cancellationToken)
    {
        var entity = await context.Players
            .Include(x => x.Country)
            .Include(x => x.Game)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Player>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Player>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Players
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Player> Add(Player player, CancellationToken cancellationToken)
    {
        await context.Players.AddAsync(player, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return player;
    }

    public async Task<Player> Update(Player player, CancellationToken cancellationToken)
    {
        context.Players.Update((player));

        await context.SaveChangesAsync(cancellationToken);

        return player;
    }


    public async Task<Player> Delete(Player player, CancellationToken cancellationToken)
    {
        context.Players.Remove(player);

        await context.SaveChangesAsync(cancellationToken);

        return player;
    }

    public async Task<Option<Player>> GetByNickname(string nickName, CancellationToken cancellationToken)
    {
        var entity = await context.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.NickName == nickName, cancellationToken);
        
        return entity == null ? Option.None<Player>() : Option.Some(entity);
    }

}