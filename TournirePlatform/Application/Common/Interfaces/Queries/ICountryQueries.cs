using Domain.Countries;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ICountryQueries
{
    Task<IReadOnlyList<Country>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Option<Country>> GetById(CountryId id, CancellationToken cancellationToken);

}