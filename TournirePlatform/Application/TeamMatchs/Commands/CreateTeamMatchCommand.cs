using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Matches.Exceptions;
using Application.TeamMatch.Exceptions;
using Domain.Countries;
using Domain.Matches;
using Domain.Teams;
using Domain.TeamsMatchs;
using Domain.Tournaments;
using MediatR;
using MatchNotFoundException = Application.TeamMatch.Exceptions.MatchNotFoundException;

namespace Application.TeamMatchs.Commands;

public class CreateTeamMatchCommand : IRequest<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>
{
    public TeamMatchId teamMatchId { get; set; }
    public TeamId TeamId { get; set; }
    public MatchId MatchId { get; set; }

    public CreateTeamMatchCommand(TeamId teamId, MatchId matchId)
    {
        TeamId = teamId;
        MatchId = matchId;
    }

    public class CreateTeamMatchCommandHandler : IRequestHandler<CreateTeamMatchCommand,
        Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>
    {
        private readonly ITeamMatchRepository _teamMatchRepository;
        private readonly IMatchQueries _matchQueries;

        public CreateTeamMatchCommandHandler(ITeamMatchRepository teamMatchRepository, IMatchQueries matchQueries)
        {
            _teamMatchRepository = teamMatchRepository;
            _matchQueries = matchQueries;
        }

        public async Task<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>> Handle(
            CreateTeamMatchCommand request, CancellationToken cancellationToken)
        {
            var existingTeamMatch =
                await _teamMatchRepository.ChekIfTeamMatchExists(request.TeamId, request.MatchId, cancellationToken);
            if (existingTeamMatch)
                throw new TeamAlreadyJoinInMatchException(Guid.NewGuid(), request.TeamId, request.MatchId);

            var matchOption = await _matchQueries.GetById(request.MatchId, cancellationToken);

            return await matchOption.Match<Task<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>>(
                async tm =>
                {
                    if (tm.TeamMatches.Count >= tm.MaxTeams)
                    {
                        throw new MatchIsAlreadyFullException(Guid.NewGuid(), request.MatchId);
                    }

                    return await CreateEntity(request.teamMatchId,  request.MatchId, request.TeamId, cancellationToken);
                },
                () => Task.FromResult<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>>(
                    new MatchNotFoundException(Guid.NewGuid(), request.MatchId)));
        }

        private async Task<Result<Domain.TeamsMatchs.TeamMatch, TeamMatchException>> CreateEntity(
            TeamMatchId teamMatchId,
            MatchId matchId,
            TeamId teamId,
            CancellationToken cancellationToken)
        {
            try
            {
                var entity = Domain.TeamsMatchs.TeamMatch.New(
                    TeamMatchId.New(),
                    teamId,
                    matchId
                );

                return await _teamMatchRepository.Add(entity, cancellationToken);
            }
            catch (Exception exception)
            {
                return new TeamMatchUnknown(TeamMatchId.Empty(), exception);
            }
        }
    }
}

