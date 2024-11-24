using System.Dynamic;
using System.Text.RegularExpressions;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Matches;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class MatchRepository : IMatchRepository, IMatchQueries
{
    private readonly ApplicationDbContext _context;

    public MatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MatchGame> Add(MatchGame matchGame, CancellationToken cancellationToken)
    {
        await _context.Matches.AddAsync(matchGame, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return matchGame;
    }

    public async Task<IReadOnlyList<MatchGame>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Matches
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<Option<MatchGame>> GetById(MatchId id, CancellationToken cancellationToken)
    {
       var entity = await _context.Matches
           .AsNoTracking()
           .Include(t => t.TeamMatches)
           .ThenInclude(t => t.Team)
           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

       return entity == null ? Option.None<MatchGame>() : Option.Some(entity);
    }

    public async Task<Option<MatchGame>> GetMatchByName(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.Matches
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == id, cancellationToken);
        
        return entity ==null ? Option.None<MatchGame>() : Option.Some(entity);
    }
    
    
    
    public async Task<MatchGame> Update(MatchGame matchGame, CancellationToken cancellationToken)
    {
        _context.Matches.Update(matchGame);
        await _context.SaveChangesAsync(cancellationToken);
        return matchGame;
    }

    public async Task<MatchGame> Delete(MatchGame match, CancellationToken cancellationToken)
    {
        _context.Matches.Remove(match);
        await _context.SaveChangesAsync(cancellationToken);
        return match;
    }
}