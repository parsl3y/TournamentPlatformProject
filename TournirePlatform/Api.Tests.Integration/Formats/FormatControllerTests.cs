using System.Net;
using System.Net.Http.Json;
using Amazon.RuntimeDependencies;
using Api.Dtos;
using Domain.Countries;
using Domain.TournamentFormat;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Data;
using Tests.Common;
using Xunit;

namespace Api.Tests.Integration.Formats;

public class FormatControllerTests (IntegrationTestWebFactory factory)
    :BaseIntegrationTest(factory), IAsyncLifetime
{
    private readonly Format _mainFormat = FormatData.MaitFormat;
    
    [Fact]
    public async Task ShouldCreateCountry()
    {
        // Arrange
        var formatName = "From Test Name";
        var request = new FormatDto(
            Id: null,
            Name: formatName);

        // Act
        var response = await Client.PostAsJsonAsync("Format/CreateFormat", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var formatFromResponse = await response.ToResponseModel<FormatDto>();
        var formatId = new FormatId(formatFromResponse.Id!.Value);

        var countryFromDataBase = await Context.Formats.FirstOrDefaultAsync(x => x.Id == formatId );
        countryFromDataBase.Should().NotBeNull();

        countryFromDataBase!.Name.Should().Be(formatName);
    }
    
    [Fact]
    public async Task ShouldNotCreateFormatBecauseNameDuplicated()
    {
        // Arrange
        var request = new FormatDto(
            Id: null,
            Name: _mainFormat.Name);

        // Act
        var response = await Client.PostAsJsonAsync("Format/CreateFormat", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    
    
    public async Task InitializeAsync()
    {
        await Context.Formats.AddAsync(_mainFormat);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Formats.RemoveRange(Context.Formats);
        await SaveChangesAsync();
    }
}

