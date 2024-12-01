namespace Domain.Teams;

public class TeamImage
{
    public TeamImageId Id { get; }

    public TeamId TeamId { get; } 
    public Team? Team { get; } 
    
    public string S3Path { get; set; }
    

    private TeamImage(TeamImageId id, TeamId teamId, string s3Path)
    {
        Id = id;
        TeamId = teamId;
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
    
    public static TeamImage New(TeamImageId id, TeamId teamId, string s3Pth)
        => new(id, teamId, s3Pth);

    public string FilePath => $"{TeamId}/{Id}.png";
}