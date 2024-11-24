using Domain.Countries;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class CountryRepository(ApplicationDbContext context) : ICountryRepositories, ICountryQueries
{
    public async Task<IReadOnlyList<Country>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Countries
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Country> Add(Country country, CancellationToken cancellationToken)
    {
        await context.Countries.AddAsync(country, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return country;
    }

    public async Task<Option<Country>> SearchByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);

        return entity == null ? Option.None<Country>() : Option.Some(entity);
    }

    public async Task<Option<Country>> GetById(CountryId id, CancellationToken cancellationToken)
    {
        var entity = await context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Country>() : Option.Some(entity);
    }

    public async Task<Country> Update(Country country, CancellationToken cancellationToken)
    {
        context.Countries.Update(country);
        await context.SaveChangesAsync(cancellationToken);
        return country;
    }
    
    public async Task<Country> Delete(Country country, CancellationToken cancellationToken)
    {
        context.Countries.Remove(country);

        await context.SaveChangesAsync(cancellationToken);

        return country;
    }
}