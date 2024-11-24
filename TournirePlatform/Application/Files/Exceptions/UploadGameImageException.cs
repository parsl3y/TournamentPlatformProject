namespace Application.Files.Exceptions;

public abstract class UploadGameImageException : Exception
{
     protected UploadGameImageException(string message) : base(message) { }
}
public class GameNotFoundException : UploadGameImageException
{
     public GameNotFoundException(Guid sneakerId) 
          : base($"Sneaker with ID {sneakerId} not found.") { }
}
public class FileUploadFailedException : UploadGameImageException
{
     public FileUploadFailedException() 
          : base("Failed to upload the file to S3.") { }
}