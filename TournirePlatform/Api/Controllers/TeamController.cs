using System.Runtime.CompilerServices;
using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Teams.Commands;
using Domain.Teams;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("Teams")]
[ApiController]
public class TeamController(ISender sender, ITeamQuery teamQuery) : ControllerBase
{
    [Route("TeamList")]
    [HttpGet]
    public async Task<IEnumerable<TeamDto>> GetAllTeams(CancellationToken cancellationToken)
    {
    
        var entities = await teamQuery.GetAllIncluded(cancellationToken);   
        return entities.Select(TeamDto.FromDomainModel).ToList();
    }

    [HttpPost("CreateTeam")]
    public async Task<ActionResult<TeamDto>> Create(
        [FromBody] TeamDtoCreate request,
        CancellationToken cancellationToken)
    {
        var input = new CreateTeamCommand
        {
            Name = request.Name,
            MatchCount = request.MatchCount,
            WinCount = request.WinCount,
            CreationDate = DateTime.UtcNow.Date
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TeamDto>>(
            t => TeamDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }

    [HttpPut("UpdateTeam")]
    public async Task<ActionResult<TeamDto>> Update([FromBody] TeamDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateTeamCommand
        {
            TeamId = request.Id!,
            Name = request.Name,
            MatchCount = request.MatchCount,
            WinCount = request.WinCount,
            WinRate = request.WinRate,
        };
        
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<TeamDto>>(
            t => TeamDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }

    [HttpDelete("DeleteTeam/{teamId}")]
    public async Task<ActionResult<TeamDto>> Delete([FromRoute] Guid teamId, CancellationToken cancellationToken)
    {
        var input = new DeleteTeamCommand()
        {
            TeamId = teamId
        };
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<TeamDto>>(
            t => TeamDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }
}