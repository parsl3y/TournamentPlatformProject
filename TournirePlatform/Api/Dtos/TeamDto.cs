using Domain.Players;
using Domain.Teams;

namespace Api.Dtos;

public record TeamDto(
    Guid Id,
    string Name,
    byte[]? Icon,
    int MatchCount,
    int WinCount,
    double WinRate,
    DateTime CreateionDate,
    List<PlayerInTeamDto> Players)
{
    public static TeamDto FromDomainModel(Team team)
    {
        
        return new TeamDto(
            Id: team.Id.Value,
            Name: team.Name,
            Icon: team.Icon,
            MatchCount: team.MatchCount,
            WinCount: team.WinCount,
            WinRate: team.WinRate,
            CreateionDate: team.CreationDate,
            Players: team.PlayerTeams.Select(PlayerInTeamDto.FromDomainModel).ToList()
        );
    }
}

public record TeamDtoCreate(
        Guid Id,
        string Name,
        byte[]? Icon,
        int MatchCount,
        int WinCount
    )
    {
        public static TeamDtoCreate FromDomainModel(Team team)
        {
            return new TeamDtoCreate(
                Id: team.Id.Value,
                Name: team.Name,
                Icon: team.Icon,
                MatchCount: team.MatchCount,
                WinCount: team.WinCount
            );
        }
    }
