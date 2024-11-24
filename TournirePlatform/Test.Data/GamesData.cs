using Domain.Countries;
using Domain.Game;

namespace Test.Data;

public class GamesData
{
    public static Game MainGame => Game.New(GameId.New(),"Main Test Game");
}