using Domain.Countries;
using Domain.Teams;

namespace Domain.Players;

public class Player
{
    public PlayerId Id { get; }
    public string NickName { get; private set; }
    public int Rating { get; private set; }
    public CountryId CountryId { get; }
    public Country? Country { get; }
    public GameId GameId { get; }
    public  Game.Game?  Game { get; }
    public byte[] Photo { get; set; }
    public DateTime UpdatedAt { get; private set; }

    public TeamId TeamId { get; private set; }
    public Teams.Team? Team { get; private set; }
    
    
    
    private Player(PlayerId id, string nickName, int rating, CountryId countryId, GameId gameId, TeamId teamId, DateTime updatedAt)
    {
        Id = id;
        NickName = nickName;
        Rating = rating;
        CountryId = countryId;
        GameId = gameId;
        TeamId = teamId;
        UpdatedAt = updatedAt;
    }

    public static Player New(PlayerId id, string nickName, int rating, CountryId countryId, GameId gameId, TeamId teamId) 
        => new(id,nickName,rating,countryId,gameId, teamId, DateTime.UtcNow);

    public void UpdateDetails(string nickName, int rating, byte[] photo)
    {
        NickName = nickName;
        Rating = rating;
        Photo = photo;
        UpdatedAt = DateTime.Now;
    }
    
}