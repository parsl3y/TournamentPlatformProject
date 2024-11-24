using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Players.Exceptions;
using Application.Tournaments.Exceptions;
using Domain.Countries;
using Domain.TournamentFormat;
using Domain.Tournaments;
using MediatR;

namespace Application.Tournaments.Commands;

public class CreateTournamentCommand: IRequest<Result<Tournament,TournamentException>>
{
    public required string Name { get; set; }
    public required DateTime StartDate { get; set; }
    public required Guid CountryId { get; set; }
    public required Guid GameId { get; set; }
    public required int PrizePool { get; set; }
    public required Guid FormatId { get; set; }
}

public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, Result<Tournament, TournamentException>>
{
    private readonly ICountryQueries _countryQueries;
    private readonly ITournamentQueries _tournamentQueries;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IGameQueries _gameQueries;
    private readonly IFormatQueries _formatQueries;

    public CreateTournamentCommandHandler(
        ICountryQueries countryQueries,
        ITournamentQueries tournamentQueries,
        ITournamentRepository tournamentRepository,
        IGameQueries gameQueries,
        IFormatQueries formatQueries)
    {
        _countryQueries = countryQueries;
        _tournamentQueries = tournamentQueries;
        _tournamentRepository = tournamentRepository;
        _gameQueries = gameQueries;
        _formatQueries = formatQueries;
    }
    
    public async Task<Result<Tournament, TournamentException>> Handle(CreateTournamentCommand request,
        CancellationToken cancellationToken)
    {
        var countryId = new CountryId(request.CountryId);
        var gameId = new GameId(request.GameId);
        var formatId = new FormatId(request.FormatId);
        
        var country = await _countryQueries.GetById(countryId, cancellationToken);
        return await country.Match(
            async c =>
                {
                    var game = await _gameQueries.GetById(gameId, cancellationToken);
                    return await game.Match(
                        async g =>
                        {
                            var format = await _formatQueries.GetById(formatId,cancellationToken);
                            return await format.Match(
                                async f =>
                                {
                                    var existingTounament =
                                        await _tournamentQueries.SearchByName(request.Name, cancellationToken);
                                    return await existingTounament.Match(
                                        t => Task.FromResult<Result<Tournament, TournamentException>>(
                                            new TournamentAlreadyExistsException(t.Id)),
                                        async () => await CreateEntity(request.Name, request.StartDate, c.Id, g.Id,
                                            request.PrizePool, f.Id, cancellationToken));
                                },
                                () => Task.FromResult<Result<Tournament, TournamentException>>(
                                    new TournamentFormatNotFoundException(formatId)));
                        },
                        () => Task.FromResult<Result<Tournament, TournamentException>>(
                            new TournamentGameNotFoundException(gameId)));
                },
                () => Task.FromResult<Result<Tournament, TournamentException>>(
                new TournamentCountryNotFoundException(countryId)));
    }

    private async Task<Result<Tournament, TournamentException>> CreateEntity(
        string name,
        DateTime startDate,
        CountryId countryId,
        GameId gameId,
        int prizePool,
        FormatId formatId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = new Tournament(TournamentId.New(), name, startDate, countryId, gameId, prizePool, formatId);

            return await _tournamentRepository.Add(entity, cancellationToken);
        }
        catch (Exception ex)
        {
            return new TournamentUnknownException(TournamentId.Empty(), ex);
        }
    }
}