using Domain.Countries;

namespace Api.Dtos;

public record CountryDto(Guid? Id, string Name)
{
    public static CountryDto FromDomainModel(Country country)
        => new (country.Id.Value, country.Name);
}