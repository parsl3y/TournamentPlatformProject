using System.ComponentModel;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Domain.Matches;
using Domain.TeamsMatchs;
using Domain.Tournaments;

namespace Api.Dtos;

public record MatchGameDtoCreate(
    Guid? Id,
    string Name,
    Guid GameId,
    DateTime StartedAt,
    int MaxTeams,
    int ScoreForGetWinner,
    Guid? TournamentId
    )
{
    public static MatchGameDtoCreate FromDomainModel(MatchGame matchGame)
        => new
        (
            Id: matchGame.Id.Value,
            Name: matchGame.Name,
            GameId: matchGame.GameId.Value,
            StartedAt: matchGame.StartAt,
            MaxTeams: matchGame.MaxTeams,
            ScoreForGetWinner: matchGame.ScoreForGetWinner,
            TournamentId: matchGame.TournamentId == null ? null : matchGame.TournamentId.Value
        );
}

public record MatchGameDto(
    string Name, // лише name залишити
    Guid GameId,
    DateTime StartedAt,
    int MaxTeams
)
{
    public static MatchGameDto FromDomainModel(MatchGame matchGame)
        => new
        (
            Name: matchGame.Name,
            GameId: matchGame.GameId.Value,
            StartedAt: matchGame.StartAt,
            MaxTeams: matchGame.MaxTeams
        );
}

public record MatchGameClearTournamentDto(
    TournamentId tournamentId
)
{
    public static MatchGameClearTournamentDto FromDomainModel(MatchGame matchGame)
        => new
        (
            tournamentId: matchGame.TournamentId
        );
}

public record MatchGameDtoInTeam(
    string Name,
    List<TeamMatchDto>  Teams
)
{
    public static MatchGameDtoInTeam FromDomainModel(MatchGame matchGame)
        => new
        (
            Name: matchGame.Name,
            Teams: matchGame.TeamMatches.Select(TeamMatchDto.FromDomainModel).ToList()
            
        );
}