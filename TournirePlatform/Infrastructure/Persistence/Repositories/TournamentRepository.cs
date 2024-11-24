using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Tournaments;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class TournamentRepository(ApplicationDbContext context) : ITournamentRepository, ITournamentQueries
{
    public async Task<Option<Tournament>> GetById(TournamentId id, CancellationToken cancellationToken)
    {
        var entity = await context.Tournaments
            .Include(x => x.matchGames)
            .ThenInclude(x => x.TeamMatches)
            .ThenInclude(x => x.Team)
            .ThenInclude(x => x.PlayerTeams)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        return entity == null ? Option.None<Tournament>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Tournament>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Tournaments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Tournament> Add(Tournament tournament, CancellationToken cancellationToken)
    {
        await context.Tournaments.AddAsync(tournament, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return tournament;
    }

    public async Task<Tournament> Update(Tournament tournament, CancellationToken cancellationToken)
    {
        context.Tournaments.Update(tournament);
        
        await context.SaveChangesAsync(cancellationToken);

        return tournament;
    }

    public async Task<Tournament> Delete(Tournament tournament, CancellationToken cancellationToken)
    {
        context.Tournaments.Remove(tournament);
        
        await context.SaveChangesAsync(cancellationToken);

        return tournament;
    }
    public async Task<Option<Tournament>> SearchByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Tournaments
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);

        return entity == null ? Option.None<Tournament>() : Option.Some(entity);
    }
}