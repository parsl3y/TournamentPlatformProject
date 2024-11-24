using Domain.Countries;
using Domain.Matches;
using Domain.Teams;
using Domain.TournamentFormat;

namespace Domain.Tournaments;

public class Tournament
{
    public TournamentId Id { get; private set; }
    public string Name { get; set; }
    public DateTime StartDate { get; private set; }
    public CountryId CountryId { get; private set; }
    public Country Country { get; private set; }
    public GameId GameId { get; private set; }
    public Game.Game Game { get; private set; }
    public int PrizePool { get; set; }
    public ICollection<MatchGame> matchGames { get; private set; } = [];
    public FormatId FormatTournamentId { get; private set; }
    public Format FormatTournament { get; private set; } //Online or Ofline

    public Tournament(TournamentId id, string name, DateTime startDate, CountryId countryId, GameId gameId,int prizePool,FormatId formatTournamentId)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        CountryId = countryId;
        GameId = gameId;
        PrizePool = prizePool;
        FormatTournamentId = formatTournamentId ;
    }
    
    public static Tournament New(TournamentId id,  string name, DateTime startDate, CountryId countryId, GameId gameId, int prizePool,FormatId formatTournamentId)
        => new Tournament(id,name, startDate, countryId, gameId, prizePool, formatTournamentId);

    public void UpdateDetails(DateTime startDate, int prizePool, FormatId formatTournamentId)
    {
        StartDate = startDate.Date;
        PrizePool = prizePool;
        FormatTournamentId = formatTournamentId; 
    }

}