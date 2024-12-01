using Application.Common.Interfaces.Repositories;
using Domain.Countries;
using Domain.Images;
using Domain.Faculties;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CountryImageRepository : ICountryImageRepository
{
    private readonly ApplicationDbContext _context;

    public CountryImageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CountryImage>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.CountryImages
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<CountryImage> Add(CountryImage countryImage, CancellationToken cancellationToken)
    {
        await _context.CountryImages.AddAsync(countryImage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return countryImage;
    }

    public async Task<CountryImage> GetById(CountryImageId id, CancellationToken cancellationToken)
    {
        return await _context.CountryImages
            .FirstOrDefaultAsync(ci => ci.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<CountryImage>> GetByCountryId(CountryId countryId, CancellationToken cancellationToken)
    {
        return await _context.CountryImages
            .Where(ci => ci.CountryId == countryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCountryId(CountryId countryId, CancellationToken cancellationToken)
    {
        return await _context.CountryImages
            .AnyAsync(image => image.CountryId == countryId, cancellationToken);
    }

    public async Task<bool> Delete(CountryImageId id, CancellationToken cancellationToken)
    {
        var countryImage = await _context.CountryImages.FirstOrDefaultAsync(ci => ci.Id == id, cancellationToken);
        if (countryImage == null)
        {
            return false;
        }

        _context.CountryImages.Remove(countryImage);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<CountryImage> Update(CountryImage countryImage, CancellationToken cancellationToken)
    {
        _context.CountryImages.Update(countryImage);
        await _context.SaveChangesAsync(cancellationToken);
        return countryImage;
    }
}
