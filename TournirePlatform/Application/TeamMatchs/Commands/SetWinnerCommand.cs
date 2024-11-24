using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Queries;
using Application.TeamMatch.Exceptions;
using Domain.Matches;
using Domain.Teams;
using MediatR;
using MatchWasFinishedException = Application.TeamMatch.Exceptions.MatchWasFinishedException;

namespace Application.TeamMatches.Commands;

public class SetWinnerCommand : IRequest<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>
{
    public Guid TeamId { get; init; }
    public Guid MatchId { get; init; }
}

public class SetWinnerCommandHandler : IRequestHandler<SetWinnerCommand, Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>
{
    private readonly IMatchQueries _matchQueries;
    private readonly ITeamMatchRepository _teamMatchRepository;

    public SetWinnerCommandHandler(IMatchQueries matchQueries, ITeamMatchRepository teamMatchRepository)
    {
        _matchQueries = matchQueries;
        _teamMatchRepository = teamMatchRepository;
    }

    public async Task<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>> Handle(SetWinnerCommand request, CancellationToken cancellationToken)
    {
        var matchId = new MatchId(request.MatchId);
        var teamId = new TeamId(request.TeamId);

        var matchOption = await _matchQueries.GetById(matchId, cancellationToken);

        return await matchOption.Match(
            async match =>
            {
                if (match.IsFinished)
                {
                    return new MatchWasFinishedException(Guid.NewGuid(), matchId);
                }

                var targetTeamMatch = match.TeamMatches.FirstOrDefault(tm => tm.TeamId == teamId);
                if (targetTeamMatch == null)
                {
                    return new TeamNotInMatchException(Guid.NewGuid(), teamId, matchId);
                }

                foreach (var teamMatch in match.TeamMatches)
                {
                    teamMatch.IsWinner = teamMatch.TeamId == teamId;
                    var team = teamMatch.Team;

                    if (team != null)
                    {
                        team.MatchCount++;

                        if ((bool)teamMatch.IsWinner)
                        {
                            team.WinCount++;
                        }

                        team.WinRate = team.MatchCount > 0
                            ? team.WinCount * 100 / team.MatchCount 
                            : 0;
                    }
                }

                match.MarkAsFinished();

                await _teamMatchRepository.UpdateRange(match.TeamMatches, cancellationToken);

                return targetTeamMatch; 
            },
            () => Task.FromResult<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>(
                new MatchInTeamMatchNotFoundException(Guid.NewGuid(), matchId)
            )
        );
    }
}


