using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Teams.Exceptions;
using Domain.Teams;
using MediatR;

namespace Application.Teams.Commands;

public class DeleteTeamCommand : IRequest<Result<Team, TeamException>>
{
    public required Guid TeamId { get; set; }
}

public class DeleteTeamCommandHandler(ITeamQuery _teamQuery, ITeamRepository _teamRepository)
    : IRequestHandler<DeleteTeamCommand, Result<Team, TeamException>>
{
    public async Task<Result<Team, TeamException>> Handle(DeleteTeamCommand request,
        CancellationToken cancellationToken)
    {
        var teamId = new TeamId(request.TeamId);
        
        var existingTeam = await _teamQuery.GetById(teamId, cancellationToken);
        
        return await existingTeam.Match<Task<Result<Team, TeamException>>>(
            async t => await DeleteEntity(t, cancellationToken),
            () => Task.FromResult<Result<Team, TeamException>>(new TeamNotFoundException(teamId)));
    }

    public async Task<Result<Team, TeamException>> DeleteEntity(Team team, CancellationToken cancellationToken)
    {
        try
        {
            return await _teamRepository.Delete(team, cancellationToken);
        }
        catch (Exception e)
        {
            return new TeamUknownException(team.Id, e);
        }
    }
}