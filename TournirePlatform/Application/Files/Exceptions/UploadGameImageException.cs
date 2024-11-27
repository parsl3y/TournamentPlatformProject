using Domain.Countries;

namespace Application.Files.Exceptions;

public abstract class UploadGameImageException : Exception
{
     protected UploadGameImageException(string message) : base(message) { }
}
public class GameNotFoundException : UploadGameImageException
{
     public GameNotFoundException(Guid gameId) 
          : base($"Sneaker with ID {gameId} not found.") { }
}
public class FileUploadFailedException : UploadGameImageException
{
     public FileUploadFailedException() 
          : base("Failed to upload the file to S3.") { }
}

public class GameAlreadyHaveAImageException : UploadGameImageException
{
     public GameAlreadyHaveAImageException(GameId gameId)
     :base("Game already have.") { }
     
}