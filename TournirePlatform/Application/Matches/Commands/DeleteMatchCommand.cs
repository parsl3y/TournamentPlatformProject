using System.Text.RegularExpressions;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Matches.Exceptions;
using Domain.Matches;
using Domain.Tournaments;
using MediatR;

namespace Application.Matches.Commands;

public class DeleteMatchCommand : IRequest<Result<MatchGame, MatchException>>
{
    public required Guid MatchId { get; set; }
}

public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand, Result<MatchGame, MatchException>>
{
    private readonly IMatchQueries _matchQueries;
    private readonly IMatchRepository _matchRepository;

    public DeleteMatchCommandHandler(IMatchQueries matchQueries, IMatchRepository matchRepository)
    {
        _matchQueries = matchQueries;
        _matchRepository = matchRepository;
    }

    public async Task<Result<MatchGame, MatchException>> Handle(DeleteMatchCommand request, CancellationToken cancellationToken)
    {
        var matchId = new MatchId(request.MatchId);

        var existingMatch = await _matchQueries.GetById(matchId, cancellationToken);

        return await existingMatch.Match<Task<Result<MatchGame, MatchException>>>(
            async match =>
            {
                if (match.TournamentId != null)
                {
                    return new MatchJoinInTournamentException(matchId);
                }

                return await DeleteEntity(match, cancellationToken);
            },
            () => Task.FromResult<Result<MatchGame, MatchException>>(new MatchNotFoundException(matchId))
        );
    }

    private async Task<Result<MatchGame, MatchException>> DeleteEntity(MatchGame match, CancellationToken cancellationToken)
    {
        try
        {
            return await _matchRepository.Delete(match, cancellationToken);
        }
        catch (Exception e)
        {
            return new MatchUknownException(match.Id, e);
        }
    }
}
