using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Countries.Exceptions;
using Domain.Countries;
using MediatR;

namespace Application.Countries.Commands;

public class CreateCountryCommand : IRequest<Result<Country, CountryException>>
{
    public required string Name { get; init; }
}

public class CreateCountryCommandHandler(
    ICountryRepositories countryRepositories) : IRequestHandler<CreateCountryCommand, Result<Country, CountryException>>
{
    public async Task<Result<Country, CountryException>> Handle(
        CreateCountryCommand request,
        CancellationToken cancellationToken)
    {
        var existingCountry = await countryRepositories.SearchByName(request.Name, cancellationToken);

        return await existingCountry.Match(
            f => Task.FromResult<Result<Country, CountryException>>(new CountryAlreadyExistsException(f.Id)),
            async () => await CreateEntity(request.Name, cancellationToken));
    }

    private async Task<Result<Country, CountryException>> CreateEntity(string name, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new Country(CountryId.New(), name);
            return await countryRepositories.Add(entity, cancellationToken);
        }
        catch (Exception e)
        {
            return new CountryUnknownException(CountryId.Empty(), e);
        }
    }
}