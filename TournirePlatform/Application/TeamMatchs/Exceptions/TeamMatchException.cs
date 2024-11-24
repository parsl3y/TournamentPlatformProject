using Domain.Matches;
using Domain.Teams;
using Domain.TeamsMatchs;

namespace Application.TeamMatch.Exceptions;

public abstract class TeamMatchException(Guid id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public Guid id { get; } = Guid.NewGuid();
}
    public class TeamAlreadyJoinInMatchException(Guid id, TeamId teamId, MatchId matchId)
        : TeamMatchException(id, $"Team with ID {teamId} i already join in match with ID {matchId}");
        
    public class TeamNotFoundException(Guid id, TeamId teamId)
    : TeamMatchException(id, $"Team with ID {teamId} not found");        
    
    public class MatchNotFoundException(Guid id, MatchId matchId)
        : TeamMatchException(id, $"Match with ID {matchId} not found");  

    public class TeamsMatchNotFoundException(Guid id, TeamId teamId)
        : TeamMatchException(id, $"Teams match with ID {teamId} not found");     
        
    public class TeamUknownMatchException(Guid id, TeamId teamId)
        : TeamMatchException(id, $"Unknown team with ID {teamId}");

    public class TeamMatchUnknown( TeamMatchId teamMatchId, Exception? innerException = null)
        : TeamMatchException( teamMatchId.Value, $"Unknown team with ID {innerException}");

    public class MatchIsAlreadyFullException(Guid id, MatchId matchId)
        : TeamMatchException(id, $"Match with ID {matchId} already is full");

    public class TeamNotInMatchException(Guid id, TeamId teamId, MatchId matchId)
        : TeamMatchException(id, $"Team with ID {teamId} not in this match {matchId}");

    public class MatchInTeamMatchNotFoundException(Guid id, MatchId matchId)
    : TeamMatchException(id, $"Match with ID {matchId} not in this team");

    public class MatchWasFinishedException(Guid id,MatchId matchId)
    : TeamMatchException(id, $"Match with ID {matchId} finished");

    public class TeamMatchNotFoundException(Guid id, TeamMatchId teamMatchId)
    :TeamMatchException (id, $"Team with ID {teamMatchId} not found");
