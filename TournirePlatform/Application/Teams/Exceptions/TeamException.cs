using Domain.Players;
using Domain.Teams;

namespace Application.Teams.Exceptions;



public abstract class TeamException(TeamId id, string message, Exception? innerException = null)
    : Exception(message,innerException)
{
    public TeamId GameId { get; } = id;
}

public class TeamNotFoundException(TeamId id) : TeamException(id, $"Game under id:{id} Not Found");
public class TeamAlreadExistsException(TeamId id): TeamException(id, $"Game already exists: {id}");
public class TeamUknownException(TeamId id, Exception innerException) 
    :TeamException(id, $"Uknown exception for the game under id:{id}", innerException);