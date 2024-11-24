using System.Xml.Schema;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.FormatTournaments.Exceptions;
using Domain.Countries;
using Domain.Game;
using Domain.TournamentFormat;
using MediatR;
using MediatR.Wrappers;
using Optional;

namespace Application.FormatTournaments.Commands;

public class UpdateFormatCommand : IRequest<Result<Format, TournamentFormatException>>
{
    public required Guid FormatId { get; set; }
    public required string Name { get; set; }
}

public class UpdateFormatCommandHandler(
    IFormatQueries _formatQueries, IFormatRepository _formatRepository) : IRequestHandler<UpdateFormatCommand, Result<Format, TournamentFormatException>>
{
    public async Task<Result<Format, TournamentFormatException>> Handle(UpdateFormatCommand request,
        CancellationToken cancellationToken)
    {
        var formatId = new FormatId(request.FormatId);
        var format = await _formatQueries.GetById(formatId, cancellationToken);

        return await format.Match(
            async f =>
            {
                var existingFormat = await CheckDuplicated(formatId, request.Name, cancellationToken);

                return await existingFormat.Match(
                    ef => Task.FromResult<Result<Format, TournamentFormatException>>(new FormatAlreadyExistsException(ef.Id)),
                    async () => await UpdateEntity(f, request.Name, cancellationToken));
            },
            () => Task.FromResult<Result<Format, TournamentFormatException>>(new FormatNotFoundException(formatId)));
    }


private async Task<Result<Format, TournamentFormatException>> UpdateEntity(
        Format format,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            format.UpdateDetails(name);

            return await _formatRepository.Update(format, cancellationToken);
        }
        catch (Exception exception)
        {
            return new FormatUnknownException(format.Id, exception);
        }
    }

    private async Task<Option<Format>> CheckDuplicated(
        FormatId formatId,
        string name,
        CancellationToken cancellationToken)
    {
        var format = await _formatRepository.SearchByName(name, cancellationToken);

        return format.Match(
            f => f.Id == formatId ? Option.None<Format>() : Option.Some(f),
            Option.None<Format>);
    }
}
