using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.FormatTournaments.Exceptions;
using Domain.TournamentFormat;
using MediatR;

namespace Application.FormatTournaments.Commands;

public class DeleteFormatCommand : IRequest<Result<Format, TournamentFormatException>>
{
    public required Guid FormatId { get; set; }    
}

public class DeleteFormatCommandHandler(IFormatQueries _formatQueries,IFormatRepository _formatRepository)
    : IRequestHandler<DeleteFormatCommand, Result<Format, TournamentFormatException>>
{
    public async Task<Result<Format, TournamentFormatException>> Handle(DeleteFormatCommand request, CancellationToken cancellationToken)
    {
        var formatId = new FormatId(request.FormatId);
        
        var existingFormat = await _formatQueries.GetById(formatId, cancellationToken);
        
        return await existingFormat.Match<Task<Result<Format, TournamentFormatException>>>(
            async f => await DeleteEntity(f, cancellationToken),
        () => Task.FromResult<Result<Format, TournamentFormatException>>(new FormatNotFoundException(formatId)));
    }
    
    public async Task<Result<Format, TournamentFormatException>> DeleteEntity(Format format, CancellationToken cancellationToken)
    {
        try
        {
            return await _formatRepository.Delete(format, cancellationToken);
        }
        catch(Exception e)
        {
            return new FormatUnknownException(format.Id, e);
        }
    }
}