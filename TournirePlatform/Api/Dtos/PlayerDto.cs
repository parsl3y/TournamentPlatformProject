using System.Runtime.CompilerServices;

namespace Api.Dtos;
using Domain.Countries;
using Domain.Players;
using Domain.Game;

public record PlayerDto(
    Guid? Id,
    string Nickname,
    int rating,
    Guid CountryId,
    CountryDto? Country,
    Guid GameId,
    GameDto? Game,
    byte[]? Photo,
    DateTime? UpdateAt,
    Guid TeamId,
    TeamDto? Team
)
{
    public static PlayerDto FromDomainModel(Player player)
        => new
    (
    Id: player.Id.Value,
    Nickname: player.NickName,
    rating: player.Rating,
    CountryId: player.CountryId.Value,
    Country: player.Country == null ? null : CountryDto.FromDomainModel(player.Country),
    GameId: player.GameId.Value,
    Game: player.Game == null ? null : GameDto.FromDomainModel(player.Game),
    Photo: player.Photo,
    UpdateAt: player.UpdatedAt,
    TeamId: player.TeamId.Value,
    Team: player.Team == null ? null : TeamDto.FromDomainModel(player.Team)
    );
}
public record PlayerCreateDto(
    Guid? Id,
    string Nickname,
    int rating,
    Guid CountryId,
    Guid GameId,
    Guid TeamId
)
{
    public static PlayerCreateDto FromDomainModel(Player player)
        => new
        (
            Id: player.Id.Value,
            Nickname: player.NickName,
            rating: player.Rating,
            CountryId: player.CountryId.Value,
            GameId: player.GameId.Value,
            TeamId: player.TeamId.Value
        );
}

public record PlayerInTeamDto(
    string Nickname,
    int rating 
)
{
    public static PlayerInTeamDto FromDomainModel(Player player)
        => new
        (
            Nickname: player.NickName,
            rating: player.Rating
        );
}