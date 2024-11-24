using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Queries;
using Application.Matches.Exceptions;
using Domain.Matches;
using MediatR;

namespace Application.Matches.Commands;

public class RemoveTournamentFromMatchCommand : IRequest<Result<Domain.Matches.MatchGame, MatchException>>
{
    public Guid MatchId { get; init; }
}

public class RemoveTournamentFromMatchCommandHandler : IRequestHandler<RemoveTournamentFromMatchCommand, Result<Domain.Matches.MatchGame, MatchException>>
{
    private readonly IMatchQueries _matchQueries;
    private readonly IMatchRepository _matchRepository;

    public RemoveTournamentFromMatchCommandHandler(IMatchQueries matchQueries, IMatchRepository matchRepository)
    {
        _matchQueries = matchQueries;
        _matchRepository = matchRepository;
    }

    public async Task<Result<MatchGame, MatchException>> Handle(RemoveTournamentFromMatchCommand request, CancellationToken cancellationToken)
    {
        var matchId = new MatchId(request.MatchId);

        var matchOption = await _matchQueries.GetById(matchId, cancellationToken);

        return await matchOption.Match(
            async match =>
            {
                if (match.IsFinished)
                {
                    return new MatchWasFinishedException(matchId);
                }

                match.TournamentId = null;

                await _matchRepository.Update(match, cancellationToken);

                return match;
            },
            () => Task.FromResult<Result<Domain.Matches.MatchGame, MatchException>>(
                new MatchNotFoundException(matchId)
            )
        );
    }
}
