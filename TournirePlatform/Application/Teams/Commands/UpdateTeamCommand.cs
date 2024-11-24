using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Teams.Exceptions;
using Domain.Teams;
using MediatR;

namespace Application.Teams.Commands;

public class UpdateTeamCommand : IRequest<Result<Team, TeamException>>
{
    public required Guid TeamId { get; set; }
    public required string Name { get; set; }
    public byte[]? Icon { get; set; }
    public required int MatchCount { get; set; }
    public required int WinCount { get; set; }
    public required double WinRate { get; set; }
}

public class UpdateTeamCommanHandler(ITeamRepository _teamRepository, ITeamQuery _teamQuery) : IRequestHandler<UpdateTeamCommand, Result<Team,TeamException>>
{
    public async Task<Result<Team, TeamException>> Handle(UpdateTeamCommand request,
        CancellationToken cancellationToken)
    {
        var teamId = new TeamId(request.TeamId);
        
        var existingTeam = await _teamQuery.GetById(teamId, cancellationToken);

        return await existingTeam.Match(
            async t => await UpdateEntity(t, request.Name, request.Icon, request.MatchCount, request.WinCount, request.WinRate,
                cancellationToken),
            () => Task.FromResult <Result<Team,TeamException>> (new TeamNotFoundException(teamId)));
    }
    private async Task<Result<Team, TeamException>> UpdateEntity(
        Team entity,
        string name,
        byte[]? icon,
        int matchCount,
        int winCount, 
        double winRate,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(name, icon, matchCount, winCount, CalculateWinCount(matchCount, winCount));
            return await _teamRepository.Update(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new TeamUknownException(entity.Id, e);
        }
    }

    private int CalculateWinCount(int matchCount, int winCount)
    {
        return matchCount > 0 ? (winCount * 100) / matchCount: 0;
    }
}