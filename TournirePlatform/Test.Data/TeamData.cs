using Domain.Teams;

namespace Test.Data;

public class TeamData
{
    public static Team MainTeam 
        => Team.New(TeamId.New(),"Main Test Team", 0,0,0, DateTime.Now);
}