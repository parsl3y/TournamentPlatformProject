using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Players.Commands;
using Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("Player")]
public class PlayerController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IPlayerQueries _playerQueries;

    public PlayerController(ISender sender, IPlayerQueries playerQueries)
    {
        _sender = sender;
        _playerQueries = playerQueries;
    }

    [HttpGet("PlayersList")]
    public async Task<ActionResult<IReadOnlyList<PlayerDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await _playerQueries.GetAll(cancellationToken);
        return entities.Select(PlayerDto.FromDomainModel).ToList();
    }

    [HttpPost("CreatePlayer")]
    public async Task<ActionResult<PlayerDto>> Create([FromBody] PlayerCreateDto request, CancellationToken cancellationToken)
    {
        var input = new CreatePlayerCommand
        {
            NickName = request.Nickname,
            Rating = request.rating,
            GameId = request.GameId,
            CountryId = request.CountryId,
            TeamId = request.TeamId
        };
        
        var result = await _sender.Send(input, cancellationToken);
        return result.Match<ActionResult<PlayerDto>>(
            p => PlayerDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }

    [HttpPut("UpDatePlayer")]
    public async Task<ActionResult<PlayerDto>> Update([FromBody] PlayerDto request, CancellationToken cancellationToken)
    {
        var input = new UpdatePlayerCommand
        {
            PlayerId = request.Id!.Value,
            NickName = request.Nickname,
            Rating = request.rating,
            Photo = request.Photo,
        };
        
        var result = await _sender.Send(input, cancellationToken);
        return result.Match<ActionResult<PlayerDto>>( 
            p => PlayerDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }
    
    [HttpDelete("DeletePlayer/{playerId}")]
    public async Task<ActionResult<PlayerDto>> Delete([FromRoute] Guid playerId, CancellationToken cancellationToken)
    {
        var input = new DeletePlayerCommand()
        {
            PlayerId = playerId
        };
        
        var result = await _sender.Send(input, cancellationToken);
        return result.Match<ActionResult<PlayerDto>>(
            p => PlayerDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }
}