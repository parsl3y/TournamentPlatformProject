using Domain.TournamentFormat;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IFormatRepository
{
    Task<Format> Add(Format game, CancellationToken cancellationToken);
    Task<Option<Format>> SearchByName(string name, CancellationToken cancellationToken);
    Task<Format> Update(Format faculty, CancellationToken cancellationToken);
    Task<Format> Delete(Format format, CancellationToken cancellationToken);
}