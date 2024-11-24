using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Matches.Commands;
using Domain.Matches;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("Matches")]
[ApiController]
public class MatchController(ISender sender, IMatchQueries matchQueries) : ControllerBase
{
    [Route("MatchList")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchGameDto>>> GetAllGames(CancellationToken cancellationToken)
    {
        var entities = await matchQueries.GetAll(cancellationToken);
        return entities.Select(MatchGameDto.FromDomainModel).ToList();
    }

    [HttpPost("CreateMatch")]
    public async Task<ActionResult<MatchGameDto>> Create(
        [FromBody] MatchGameDtoCreate request,
        CancellationToken cancellationToken)
    {
        var input = new CreateMatchCommand
        {
            Name = request.Name,
            GameId = request.GameId,
            StartAt = DateTime.UtcNow,
            MaxTeams = request.MaxTeams,
            scoreForGetWinner = request.ScoreForGetWinner,
            TournamentId = request.TournamentId, 
        };
        
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<MatchGameDto>>(
            mg => MatchGameDto.FromDomainModel(mg),
            e => e.ToObjectResult());
    }
    
    
    [HttpPatch("RemoveTournament/{matchId:guid}")] // це як пут,але тільки для одного поля зміни
    public async Task<IActionResult> RemoveTournamentFromMatch(
        Guid matchId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTournamentFromMatchCommand
        {
            MatchId = matchId
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            match => Ok(MatchGameClearTournamentDto.FromDomainModel(match)),
            error => error.ToObjectResult());
    }

    [HttpDelete("DeletMatch")]
    public async Task<ActionResult<MatchGameDto>> Delete([FromQuery] Guid matchId, CancellationToken cancellationToken)
    {
        var input = new DeleteMatchCommand()
        {
            MatchId = matchId
        };
        
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<MatchGameDto>>(
            m => MatchGameDto.FromDomainModel(m),
            e => e.ToObjectResult());
    }
}