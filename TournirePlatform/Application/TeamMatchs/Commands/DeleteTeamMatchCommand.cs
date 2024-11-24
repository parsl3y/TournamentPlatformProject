using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.TeamMatch.Exceptions;
using Domain.TeamsMatchs;
using MediatR;

namespace Application.TeamMatchs.Commands;

public class DeleteTeamMatchCommand : IRequest<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>
{
    public required Guid TeamMatchId { get; set; }
}

public class DeleteTeamMatchCommandHandler : IRequestHandler<DeleteTeamMatchCommand, Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>
{
    private readonly ITeamMatchQuery _teamMatchQuery;
    private readonly ITeamMatchRepository _teamMatchRepository;

    public DeleteTeamMatchCommandHandler(ITeamMatchQuery teamMatchQuery, ITeamMatchRepository teamMatchRepository)
    {
        _teamMatchQuery = teamMatchQuery;
        _teamMatchRepository = teamMatchRepository;
    }

    public async Task<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>> Handle(DeleteTeamMatchCommand request,
        CancellationToken cancellationToken)
    {
        var teamMatchId = new TeamMatchId(request.TeamMatchId);

        var existingTeamMatch = await _teamMatchQuery.GetById(teamMatchId, cancellationToken);

        return await existingTeamMatch.Match<Task<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>>(
            async tm => await DeleteEntity(tm, cancellationToken),
            () => Task.FromResult<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>(new TeamMatchNotFoundException(Guid.NewGuid(),teamMatchId)));
    }

    private async Task<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>> DeleteEntity(Domain.TeamsMatchs.TeamMatch teamMatch, CancellationToken cancellationToken)
    {
        try
        {
            return await _teamMatchRepository.Delete(teamMatch, cancellationToken);
        }
        catch (Exception e)
        {
            return new TeamMatchUnknown(teamMatch.Id, e);
        }
    }
}