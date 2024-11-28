using Domain.Countries;
using Domain.Images;
namespace Application.Common.Interfaces.Repositories;

public interface ICountryImageRepository
{
    Task<IReadOnlyList<CountryImage>> GetAll(CancellationToken cancellationToken);
    Task<CountryImage> Add(CountryImage countryImage, CancellationToken cancellationToken);
    Task<bool> ExistsByCountryId(CountryId countryId, CancellationToken cancellationToken);
    Task<CountryImage> GetById(CountryImageId id, CancellationToken cancellationToken);
    Task<IEnumerable<CountryImage>> GetByCountryId(CountryId countryId, CancellationToken cancellationToken);
    Task<bool> Delete(CountryImageId id, CancellationToken cancellationToken);
    Task<CountryImage> Update(CountryImage countryImage, CancellationToken cancellationToken);
}