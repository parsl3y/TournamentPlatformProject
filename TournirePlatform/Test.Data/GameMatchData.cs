using Domain.Countries;
using Domain.Matches;
using Domain.Players;
using Domain.Tournaments;

namespace Test.Data;

public static class GameMatchData
{
    public static MatchGame MainGameMatch(GameId gameId, TournamentId tournamentId)
        => MatchGame.New(MatchId.New(),"name match", gameId ,DateTime.UtcNow,  0, 0, tournamentId);
}