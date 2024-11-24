using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Countries.Exceptions;
using Domain.Countries;
using MediatR;
using Optional;

namespace Application.Countries.Commands;

public record UpdateCountryCommand : IRequest<Result<Country, CountryException>>
{
    public required Guid CountryId { get; init; }
    public required string Name { get; init; }
}

public class UpdateCountryCommandHandler(
    ICountryRepositories countryRepositories, ICountryQueries countryQueries) : IRequestHandler<UpdateCountryCommand, Result<Country, CountryException>>
{
    public async Task<Result<Country, CountryException>> Handle(UpdateCountryCommand request,
        CancellationToken cancellationToken)
    {
        var countryId = new CountryId(request.CountryId);
        var country = await countryQueries.GetById(countryId, cancellationToken);

        return await country.Match(
            async c =>
            {
                var existingCountry = await CheckDuplicated(countryId, request.Name, cancellationToken);

                return await existingCountry.Match(
                    ec => Task.FromResult<Result<Country, CountryException>>(new CountryAlreadyExistsException(ec.Id)),
                    async () => await UpdateEntity(c, request.Name, cancellationToken));
            },
            () => Task.FromResult<Result<Country, CountryException>>(new CountryNotFoundException(countryId)));
    }

    private async Task<Result<Country, CountryException>> UpdateEntity(
        Country country,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            country.UpdateDetails(name);
            return await countryRepositories.Update(country, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CountryUnknownException(country.Id, exception);
        }
    }

    private async Task<Option<Country>> CheckDuplicated(
        CountryId countryId,
        string name,
        CancellationToken cancellationToken)
    {
        var country = await countryRepositories.SearchByName(name, cancellationToken);

        return country.Match(
            c => c.Id == countryId ? Option.None<Country>() : Option.Some(c),
            Option.None<Country>);
    }
}
