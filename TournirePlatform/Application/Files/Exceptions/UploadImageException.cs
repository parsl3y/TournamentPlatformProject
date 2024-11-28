using Domain.Countries;
using Domain.Players;
using Domain.Teams;

namespace Application.Files.Exceptions;

public abstract class UploadImageException : Exception
{
     protected UploadImageException(string message) : base(message) { }
}
public class NotFoundException : UploadImageException
{
     public NotFoundException(Guid gameId) 
          : base($"Sneaker with ID {gameId} not found.") { }
}
public partial class FileUploadFailedException : UploadImageException
{
     public FileUploadFailedException() 
          : base("Failed to upload the file to S3.") { }
}

public partial class PlayerAlreadyHasAnImageException : UploadImageException
{
     public PlayerAlreadyHasAnImageException()
          : base("The player has already been added.") { }
}

public class AlreadyHaveAImageException : UploadImageException
{
     public AlreadyHaveAImageException(GameId gameId)
     :base("Game already have.") { }
     
}
public class AlreadyHavePlayerImageException : UploadImageException
{
     public AlreadyHavePlayerImageException(PlayerId PlayerId)
          :base("Player already have.") { }
     
}
public class AlreadyHaveCountryImageException : UploadImageException
{
     public AlreadyHaveCountryImageException(CountryId countryId)
          :base("Country already have.") { }
     
}
public class AlreadyHaveTeamImageException : UploadImageException
{
     public AlreadyHaveTeamImageException(TeamId TeamId)
          :base("Team already have.") { }
     
}