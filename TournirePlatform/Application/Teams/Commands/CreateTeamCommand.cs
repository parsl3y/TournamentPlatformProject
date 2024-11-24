using System.Runtime.InteropServices.JavaScript;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Teams.Exceptions;
using Domain.Matches;
using Domain.Players;
using Domain.Teams;
using MediatR;
using MediatR.Wrappers;

namespace Application.Teams.Commands;

public class CreateTeamCommand : IRequest<Result<Team, TeamException>>
{
    public required string Name { get; set; }
    public byte[]? Icon { get; set; }
    public required int MatchCount { get; set; }
    public required int WinCount { get; set; }
    public required DateTime CreationDate { get; set; }
    
    
    public class CreateTeamCommandHandler(
        ITeamRepository _teamRepository,
        ITeamQuery _teamQuery)
        : IRequestHandler<CreateTeamCommand, Result<Team, TeamException>>
    {
        public async Task<Result<Team, TeamException>> Handle(CreateTeamCommand request,
            CancellationToken cancellationToken)
        {
            var existingTeam = await _teamQuery.SearchByName(request.Name, cancellationToken);

            return await existingTeam.Match(
                t => Task.FromResult<Result<Team, TeamException>>(new TeamAlreadExistsException(t.Id)),
                async () => await CreateEntity(request.Name, request.Icon, request.MatchCount, request.WinCount,
                    cancellationToken));
        }

        private async Task<Result<Team, TeamException>> CreateEntity(string name, byte[]? icon, int matchCount,
            int winCount, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new Team(
                    TeamId.New(),
                    name,
                    icon,
                    matchCount,
                    winCount,
                    CalculateWinRate(matchCount, winCount),
                    DateTime.UtcNow
                );
                return await _teamRepository.Add(entity, cancellationToken);
            }
            catch (Exception e)
            {
                return new TeamUknownException(TeamId.Empty(), e);
            }
        }

        private int CalculateWinRate(int matchCount, int winCount)
        {
            return matchCount > 0 ? (winCount * 100) / matchCount : 0;
        }
    }
}