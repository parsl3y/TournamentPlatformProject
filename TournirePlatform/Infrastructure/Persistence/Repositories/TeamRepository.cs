using System.Runtime.InteropServices;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Players;
using Domain.Teams;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class TeamRepository(ApplicationDbContext context) : ITeamRepository, ITeamQuery
{
    public async Task<IReadOnlyList<Team>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Teams
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Team>> GetAllIncluded(CancellationToken cancellationToken)
    {
        return await context.Teams
            .Include(x => x.PlayerTeams)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Option<Team>> GetById(TeamId id, CancellationToken cancellationToken)
    {
        var entity = await context.Teams
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Team>() : Option.Some (entity);
    }
    
    public async Task<Option<Team>> SearchByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Teams
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
        
        return entity == null ? Option.None<Team>() : Option.Some(entity);
    }
    
    public async Task<Team> Add(Team team, CancellationToken cancellationToken)
    {
        await context.Teams.AddAsync(team, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return team;
    }
    
    public async Task<Team> Update(Team team, CancellationToken cancellationToken)
    {
        context.Teams.Update(team);

        await context.SaveChangesAsync(cancellationToken);

        return team;
    }

    public async Task<Team> Delete(Team team, CancellationToken cancellationToken)
    {
        context.Teams.Remove(team);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return team;
    }
}