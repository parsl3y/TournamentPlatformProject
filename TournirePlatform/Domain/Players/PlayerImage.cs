namespace Domain.Players;

public class PlayerImage
{
    public PlayerImageId Id { get; }

    public PlayerId PlayerId { get; } 
    public Player? Player { get; } 
    
    public string S3Path { get; set; }
    

    private PlayerImage(PlayerImageId id, PlayerId playerId, string s3Path)
    {
        Id = id;
        PlayerId= playerId;
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
    
    public static PlayerImage New(PlayerImageId id, PlayerId playerId, string s3Pth)
        => new(id, playerId, s3Pth);

    public string FilePath => $"{PlayerId}/{Id}.png";
}