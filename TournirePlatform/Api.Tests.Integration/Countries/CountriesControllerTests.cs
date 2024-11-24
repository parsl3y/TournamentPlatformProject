 using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Countries;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Data;
using Tests.Common;
using Xunit;

namespace Api.Tests.Integration.Games;

public class CountriesControllerTests(IntegrationTestWebFactory factory)
    : BaseIntegrationTest(factory), IAsyncLifetime
{
    private readonly Country _mainCountry = CountryData.MainCountry;

    [Fact]
    public async Task ShouldCreateCountry()
    {
        // Arrange
        var countryName = "From Test Name";
        var request = new CountryDto(
            Id: null,
            Name: countryName);

        // Act
        var response = await Client.PostAsJsonAsync("Country", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var countryFromResponse = await response.ToResponseModel<GameDto>();
        var countryId = new CountryId(countryFromResponse.Id!.Value);

        var countryFromDataBase = await Context.Countries.FirstOrDefaultAsync(x => x.Id == countryId);
        countryFromDataBase.Should().NotBeNull();

        countryFromDataBase!.Name.Should().Be(countryName);
    }

    [Fact]
    public async Task ShouldUpdatCountry()
    {
        // Arrange
        var newCountryName = "New Country Name";
        var request = new CountryDto(
            Id: _mainCountry.Id.Value,
            Name: newCountryName);

        // Act
        var response = await Client.PutAsJsonAsync("Country", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var countryFromResponse = await response.ToResponseModel<CountryDto>();

        var countryFromDataBase = await Context.Countries
            .FirstOrDefaultAsync(x => x.Id == new CountryId(countryFromResponse.Id!.Value));

        countryFromDataBase.Should().NotBeNull();

        countryFromDataBase!.Name.Should().Be(newCountryName);
    }

    [Fact]
    public async Task ShouldNotCreateCountryBecauseNameDuplicated()
    {
        // Arrange
        var request = new CountryDto(
            Id: null,
            Name: _mainCountry.Name);

        // Act
        var response = await Client.PostAsJsonAsync("Country", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotUpdateCountryBecauseFacultyNotFound()
    {
        // Arrange
        var request = new CountryDto(
            Id: Guid.NewGuid(),
            Name: "New Country Name");

        // Act
        var response = await Client.PutAsJsonAsync("Country", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync()
    {
        await Context.Countries.AddAsync(_mainCountry);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Countries.RemoveRange(Context.Countries);

        await SaveChangesAsync();
    }
}