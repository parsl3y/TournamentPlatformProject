using Domain.Countries;

namespace Application.Games.Exceptions;

public abstract class GameException(GameId id, string message, Exception? innerException = null)
  : Exception(message,innerException)
{
    public GameId GameId { get; } = id;
}

public class GameNotFoundException(GameId id) : GameException(id, $"Game under id:{id} Not Found");
public class GameAlreadExistsException(GameId id): GameException(id, $"Game already exists: {id}");
public class GameUknownException(GameId id, Exception innerException) 
    :GameException(id, $"Uknown exception for the game under id:{id}", innerException);