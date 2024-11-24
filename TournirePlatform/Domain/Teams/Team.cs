using Domain.Players;
using Domain.TeamsMatchs;
using Domain.Teams;

namespace Domain.Teams;

public class Team
{
    public TeamId Id { get; init; }
    public string Name { get; set; }
    public byte[]? Icon { get; set; }
    public int MatchCount { get; set; }
    public int WinCount { get; set; }
    public double WinRate { get; set; }  
    public DateTime CreationDate { get; init; }
    
    public List<Player> PlayerTeams { get; private set; } = new List<Player>();
    public ICollection<TeamMatch> TeamMatches { get; private set; } = [];

    public Team(TeamId id, string name, byte[] icon, int matchCount, int winCount, double winRate, DateTime creationDate)
    {
        Id = id;
        Name = name;
        Icon = icon;
        MatchCount = matchCount;
        WinCount = winCount;
        WinRate = winRate;
        CreationDate = creationDate;
    }
    
    public static Team New(TeamId id, string name, byte[] icon, int matchCount, int winCount, double winRate, DateTime creationDate)
        => new Team(id, name, icon, matchCount, winCount, winRate, creationDate);

    public void UpdateDetails(string name, byte[] icon, int matchCount, int winCount, double winRate) 
    {
        Name = name;
        Icon = icon;
        MatchCount = matchCount;
        WinCount = winCount;
        WinRate = winRate;
    }
}