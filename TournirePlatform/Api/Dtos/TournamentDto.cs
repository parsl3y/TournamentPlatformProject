using Domain.Countries;
using Domain.TournamentFormat;
using Domain.Tournaments;
using Optional;

namespace Api.Dtos;

public record TournamentDto(
    string Name,
    DateTime? startDate,
    CountryId CountryId, 
    int prizePool,
    string Format,
    List<MatchGameDtoInTeam> Matches

    )
{
    public static TournamentDto FromDomainModel (Tournament tournament)
            => new
        (
            Name: tournament.Name,
            startDate: tournament.StartDate,
            CountryId: tournament.CountryId,
            Format: tournament.FormatTournament.Name,
            prizePool: tournament.PrizePool,
            Matches: tournament.matchGames.Select(MatchGameDtoInTeam.FromDomainModel).ToList()
       
        );
    
}
public record TournamentCreateDto(  
    string Name,
    DateTime startDate,
    CountryId? CountryId, 
    GameId? GameId,
    int prizePool,
    FormatId? FormatId
    )
{

    public static TournamentCreateDto FromDomainModel (Tournament tournament)
       => new (
           Name: tournament.Name,
           startDate: tournament.StartDate,
           CountryId: tournament.CountryId,
           GameId: tournament.GameId,
           prizePool: tournament.PrizePool,
           FormatId: tournament.FormatTournamentId
        );
    
}