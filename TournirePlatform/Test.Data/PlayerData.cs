using Domain.Countries;
using Domain.Players;
using Domain.Teams;

namespace Test.Data;

public static class PlayerData
{
    public static Player MainPlayer(CountryId countryId, GameId gameId, TeamId teamId)
        => Player.New(PlayerId.New(), "Player Nick Name", 100,countryId, gameId, teamId);
    
    public static Player ExtraPlayer(CountryId countryId, GameId gameId, TeamId teamId)
        => Player.New(PlayerId.New(), "Player Nick Name", 100,countryId, gameId, teamId);
}