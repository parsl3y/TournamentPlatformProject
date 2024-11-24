 using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Countries;
using Domain.Game;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Data;
using Tests.Common;
using Xunit;

namespace Api.Tests.Integration.Games;

public class GamesControllerTests(IntegrationTestWebFactory factory)
    : BaseIntegrationTest(factory), IAsyncLifetime
{
    private readonly Game _mainGame = GamesData.MainGame;

    [Fact]
    public async Task ShouldCreateGame()
    {
        // Arrange
        var gameName = "From Test Name";
        var request = new GameDto(
            Id: null,
            Name: gameName);

        // Act
        var response = await Client.PostAsJsonAsync("Games", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var gameFromResponse = await response.ToResponseModel<GameDto>();
        var gameId = new GameId(gameFromResponse.Id!.Value);

        var gameFromDataBase = await Context.Games.FirstOrDefaultAsync(x => x.Id == gameId);
        gameFromDataBase.Should().NotBeNull();

        gameFromDataBase!.Name.Should().Be(gameName);
    }

    [Fact]
    public async Task ShouldUpdatGame()
    {
        // Arrange
        var newGameName = "New Game Name";
        var request = new GameDto(
            Id: _mainGame.Id.Value,
            Name: newGameName);

        // Act
        var response = await Client.PutAsJsonAsync("Games", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var gameFromResponse = await response.ToResponseModel<GameDto>();

        var gameyFromDataBase = await Context.Games
            .FirstOrDefaultAsync(x => x.Id == new GameId(gameFromResponse.Id!.Value));

        gameyFromDataBase.Should().NotBeNull();

        gameyFromDataBase!.Name.Should().Be(newGameName);
    }

    [Fact]
    public async Task ShouldNotCreateGameBecauseNameDuplicated()
    {
        // Arrange
        var request = new GameDto(
            Id: null,
            Name: _mainGame.Name);

        // Act
        var response = await Client.PostAsJsonAsync("Games", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotUpdateGameBecauseFacultyNotFound()
    {
        // Arrange
        var request = new GameDto(
            Id: Guid.NewGuid(),
            Name: "New Game Name");

        // Act
        var response = await Client.PutAsJsonAsync("Games", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public async Task InitializeAsync()
    {
        await Context.Games.AddAsync(_mainGame);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Games.RemoveRange(Context.Games);

        await SaveChangesAsync();
    }
}