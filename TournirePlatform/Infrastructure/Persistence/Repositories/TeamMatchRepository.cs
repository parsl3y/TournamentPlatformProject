using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Matches;
using Domain.Players;
using Domain.TeamsMatchs;
using Domain.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class TeamMatchRepository : ITeamMatchRepository, ITeamMatchQuery
{
    private readonly ApplicationDbContext _conntext;

    public TeamMatchRepository(ApplicationDbContext conntext)
    {
        _conntext = conntext;
    }

    public async Task<bool> ChekIfTeamMatchExists(TeamId teamId, MatchId matchId, CancellationToken cancellationToken)
    {
        return await _conntext.TeamMatches
            .AsNoTracking()
            .AnyAsync(tm => tm.TeamId == teamId && tm.MatchId == matchId, cancellationToken);
    }
    
    public async Task<TeamMatch> Add(TeamMatch teamMatch, CancellationToken cancellationToken)
    {
        await _conntext.TeamMatches.AddAsync(teamMatch, cancellationToken);
        await _conntext.SaveChangesAsync(cancellationToken);
        
        return teamMatch;
    }
    public async Task UpdateRange(IEnumerable<TeamMatch> teamMatches, CancellationToken cancellationToken)
    {
        _conntext.TeamMatches.UpdateRange(teamMatches);
        await _conntext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TeamMatch> Delete(TeamMatch teamMatch, CancellationToken cancellationToken)
    {
        _conntext.TeamMatches.RemoveRange(teamMatch);
        
        await _conntext.SaveChangesAsync(cancellationToken);

        return teamMatch;
    }

    public async Task<Option<TeamMatch>> GetById(TeamMatchId id, CancellationToken cancellationToken)
    {
        var entity = await _conntext.TeamMatches
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        return entity == null ? Option.None<TeamMatch>() : Option.Some(entity);
    }
}