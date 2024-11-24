using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.FormatTournaments.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route ("Format")]
[ApiController]
public class FormatController(ISender sender, IFormatQueries _formatQueries) : ControllerBase
{
    [HttpGet("FormatList")]
    public async Task<ActionResult<IEnumerable<FormatDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await _formatQueries.GetAll(cancellationToken);
        return entities.Select(FormatDto.FromDomainModle).ToList();
    }
    
    [HttpPost("CreateFormat")]
    public async Task<ActionResult<FormatDto>> Create(
        [FromBody] FormatDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateFormatCommand()
        {
            Name = request.Name
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<FormatDto>>(
            f => FormatDto.FromDomainModle(f),
            e => e.ToObjectResult());
    }

    [HttpDelete("DeleteFormat")]
    public async Task<ActionResult<FormatDto>> Delete([FromQuery] Guid formatId, CancellationToken cancellationToken)
    {
        var input = new DeleteFormatCommand()
        {
        FormatId = formatId
        };
        
        var result = await sender.Send(input, cancellationToken);
        return result.Match<ActionResult<FormatDto>>(
        f => FormatDto.FromDomainModle(f),
            e => e.ToObjectResult());
    }
    
    
}