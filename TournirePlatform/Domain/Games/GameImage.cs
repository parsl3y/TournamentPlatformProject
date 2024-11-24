using Domain.Countries;

namespace Domain.Faculties;

public class GameImage
{
    public GameImageId Id { get; }

    public GameId GameId { get; } 
    public Game.Game? Game { get; } 
    
    public string S3Path { get; set; }
    

    private GameImage(GameImageId id, GameId gameId, string s3Path)
    {
        Id = id;
        GameId = gameId;
        S3Path = s3Path;  
    }
    
    public void UpdateImageUrl(string newS3Path)
    {
        if (string.IsNullOrWhiteSpace(newS3Path))
        {
            throw new ArgumentException("New image URL cannot be empty", nameof(newS3Path));
        }

        S3Path = newS3Path;
    }
    
    public static GameImage New(GameImageId id, GameId gameId, string s3Pth)
        => new(id, gameId, s3Pth);

    public string FilePath => $"{GameId}/{Id}.png";
}