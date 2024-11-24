using Domain.Countries;
using Domain.Game;

namespace Api.Dtos;

public record GameDto(Guid? Id, string Name)
{
    public static GameDto FromDomainModel(Game game)
        => new (game.Id.Value, game.Name);
}