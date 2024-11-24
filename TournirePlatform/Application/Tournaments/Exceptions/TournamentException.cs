using Domain.Countries;
using Domain.Game;
using Domain.TournamentFormat;
using Domain.Tournaments;

namespace Application.Tournaments.Exceptions;

public abstract class TournamentException(TournamentId id, string message, Exception? innerException = null)
    :Exception(message,innerException)
{
    public TournamentId TournamentId { get; } = id;
}

    public class TournamentNotFoundException(TournamentId id) 
        : TournamentException(id, "Tournament not found");
    
    public class TournamentAlreadyExistsException(TournamentId id)
        : TournamentException(id, "Tournament already exists");
        
    public class TournamentUnknownException(TournamentId id, Exception? innerException)
        : TournamentException(id, $"Uknown exception for the game under id:{id}", innerException);
        
   public class TournamentCountryNotFoundException(CountryId countryId)
        : TournamentException(TournamentId.Empty(), $"PlayerCountry {countryId} found");

   public class TournamentGameNotFoundException(GameId gameId)
       : TournamentException(TournamentId.Empty(), $"PlayerCountry {gameId} found"); 

   public class TournamentFormatNotFoundException(FormatId formatId)
       : TournamentException(TournamentId.Empty(), $"PlayerCountry {formatId} found"); 
   