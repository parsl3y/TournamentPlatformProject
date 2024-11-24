using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Games.Commands;
using Domain.Game;
using EFCore.NamingConventions.Internal;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("Games")]
[ApiController]
public class GamesControllers(ISender sender, IGameQueries gameQueries) : ControllerBase
{
    [Route("GamesList")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGames(CancellationToken cancellationToken)
    {
        var entities = await gameQueries.GetAll(cancellationToken);
        return entities.Select(GameDto.FromDomainModel).ToList();
    }
    
    [HttpPost("CreateGame")]
    public async Task<ActionResult<GameDto>> Create(
        [FromBody] GameDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateGameCommand
        {
            Name = request.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<GameDto>>(
            f => GameDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpPut("UpdateGame")]
    public async Task<ActionResult<GameDto>> Update(
        [FromBody] GameDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateGameCommand()
        {
            GameId = request.Id!.Value,
            Name = request.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<GameDto>>(
            f => GameDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("DeleteGame/{gameId}")]
    public async Task<ActionResult<GameDto>> Delete([FromRoute] Guid gameId, CancellationToken cancellationToken)
    {
        var input = new DeleteGameCommand()
        {
            GameId = gameId
        };
        
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<GameDto>>(
            p => GameDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }
}