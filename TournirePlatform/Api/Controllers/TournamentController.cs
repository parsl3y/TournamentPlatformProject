using System.Runtime.InteropServices.JavaScript;
using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Tournaments.Commands;
using Domain.Tournaments;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Api.Controllers;


[Route("Tournament")]
[ApiController]
public class TournamentController(ISender sender, ITournamentQueries _tournamentQueries)
{
    [HttpGet("TournamentsList")]
    public async Task<List<TournamentCreateDto>> GetAllTournaments(CancellationToken cancellationToken)
    {
        var entities = await _tournamentQueries.GetAll(cancellationToken);
        return entities.Select(TournamentCreateDto.FromDomainModel).ToList();
    }

    [HttpPost("CreateTournament")]
    public async Task<ActionResult<TournamentCreateDto>> Create([FromBody]TournamentCreateDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateTournamentCommand
        {
            Name = request.Name,
            StartDate = request.startDate,
            CountryId = request.CountryId.Value,
            GameId = request.GameId.Value,
            PrizePool = request.prizePool,
            FormatId = request.FormatId.Value
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<TournamentCreateDto>>(
            t => TournamentCreateDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }

    [HttpGet("GetTournament/{id:guid}")]
    public async Task<ActionResult<TournamentDto>> Get([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var entity = await _tournamentQueries.GetById(new TournamentId(id), cancellationToken);

        return entity.Match<TournamentDto>(
            success => TournamentDto.FromDomainModel(success),
            () => TournamentDto.FromDomainModel(null));
    }
}