using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Queries;
using Application.Matches.Exceptions;
using Application.TeamMatch.Exceptions;
using Domain.Matches;
using Domain.Teams;
using Domain.TeamsMatchs;
using MediatR;
using Optional;
using MatchWasFinishedException = Application.TeamMatch.Exceptions.MatchWasFinishedException;

namespace Application.TeamMatches.Commands;

public class AddScoreCommand : IRequest<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>
{
    public Guid TeamId { get; init; }
    public Guid MatchId { get; init; }
}

public class AddScoreCommandHandler : IRequestHandler<AddScoreCommand, Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>
{
    private readonly IMatchQueries _matchQueries;
    private readonly ITeamMatchRepository _teamMatchRepository;

    public AddScoreCommandHandler(IMatchQueries matchQueries, ITeamMatchRepository teamMatchRepository)
    {
        _matchQueries = matchQueries;
        _teamMatchRepository = teamMatchRepository;
    }

    public async Task<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>> Handle(AddScoreCommand request, CancellationToken cancellationToken)
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


                targetTeamMatch.Score++;


                if (targetTeamMatch.Score >= match.ScoreForGetWinner)
                {
                    foreach (var teamMatch in match.TeamMatches)
                    {
                        teamMatch.IsWinner = teamMatch.TeamId == teamId;
                    }


                    match.MarkAsFinished();
                }


                await _teamMatchRepository.UpdateRange(match.TeamMatches, cancellationToken);

                return targetTeamMatch;
            },
            () => Task.FromResult<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>(
                new MatchInTeamMatchNotFoundException(Guid.NewGuid(), matchId)
            )
        );
    }
}
