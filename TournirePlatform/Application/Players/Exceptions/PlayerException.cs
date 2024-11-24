using Domain.Countries;
using Domain.Players;
using Domain.Teams;

namespace Application.Players.Exceptions;

public class PlayerException : Exception
{
    public PlayerId PlayerId { get; }

    public PlayerException(PlayerId id, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        PlayerId = id;
    }
}

    public class PlayerNotFoundException : PlayerException
    {
        public PlayerNotFoundException(PlayerId id)
            : base(id, $"Player with id {id} was not found.") { }
    }

    public class PlayerAlreadyExistsException : PlayerException
    {
        public PlayerAlreadyExistsException(PlayerId id)
            :base(id, $"Player with id {id} is already exists.") { }
    }

    public class PlayerCountryNotFoundException : PlayerException
    {
        public PlayerCountryNotFoundException(CountryId countryId)
            :base(PlayerId.Empty(), $"Country with id {countryId} was not found.") { }
    }

    public class PlayerGameNotFoundException : PlayerException
    {
        public PlayerGameNotFoundException(GameId gameId)
            :base(PlayerId.Empty(), $"Game with id {gameId} was not found.") { }
    }
    public class PlayerTeamNotFoundException : PlayerException
    {
        public PlayerTeamNotFoundException(TeamId teamId)
            :base(PlayerId.Empty(), $"Game with id {teamId} was not found.") { }
    }
    public class PlayerUknownException : PlayerException
    {
        public PlayerUknownException(PlayerId id, Exception? innerException)
            : base(id, $"Unknown exception for the player under id: {id}", innerException){}
    }
