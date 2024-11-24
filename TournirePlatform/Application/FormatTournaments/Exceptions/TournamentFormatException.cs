using Domain.TournamentFormat;

namespace Application.FormatTournaments.Exceptions;

public abstract class TournamentFormatException(FormatId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public FormatId FormatId { get; } = id;
}

public class FormatNotFoundException(FormatId id) 
    : TournamentFormatException(id, $"Format under id:{id} Not Found");

public class FormatAlreadyExistsException(FormatId id) 
    : TournamentFormatException(id, $"Format under id:{id} Already Exists");

public class FormatUnknownException(FormatId id, Exception innerException)
    : TournamentFormatException(id, $"Unknown format under id:{id}", innerException);