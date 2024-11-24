using Domain.Countries;
using Domain.TournamentFormat;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IFormatQueries
{
    Task<IReadOnlyList<Format>> GetAll(CancellationToken cancellationToken);
    Task<Option<Format>> GetById(FormatId id, CancellationToken cancellationToken);
}