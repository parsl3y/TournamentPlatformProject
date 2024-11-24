using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Countries.Exceptions;
using Domain.Countries;
using Domain.Game;
using Domain.TournamentFormat;
using MediatR;

namespace Application.Countries.Commands;

public class DeleteCountryCommand : IRequest<Result<Country, CountryException>>
{
    public required Guid CountryId { get; set; }
}

public class DeleteCountryCommandHandler(ICountryQueries _countryQueries, ICountryRepositories _countryRepositories) 
    : IRequestHandler<DeleteCountryCommand, Result<Country, CountryException>>
{
    public async Task<Result<Country, CountryException>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var countryId = new CountryId(request.CountryId);
        
        var existingCountry = await _countryQueries.GetById(countryId, cancellationToken);
        
        return await existingCountry.Match<Task<Result<Country, CountryException>>>(
            async c => await DeleteEntity(c, cancellationToken),
            () => Task.FromResult<Result<Country, CountryException>>(new CountryNotFoundException(countryId)));
    }
    public async Task<Result<Country, CountryException>> DeleteEntity(Country country, CancellationToken cancellationToken)
    {
        try
        {
            return await _countryRepositories.Delete(country, cancellationToken);
        }
        catch(Exception e)
        {
            return new CountryUnknownException(country.Id, e);
        }
    }
}