using System.Net.Http.Json;
using Api.Dtos;
using Domain.Countries;
using Domain.Game;
using Domain.Players;
using Domain.Teams;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;
using Test.Data;
using Tests.Common;
using Xunit;

namespace Api.Tests.Integration.Players;

public class PlayersControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Country _mainCountry = CountryData.MainCountry;
    private readonly Game _mainGame = GamesData.MainGame;
    private readonly Team _mainTeam = TeamData.MainTeam;
    private readonly Player _mainPlayer;

    public PlayersControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainPlayer = PlayerData.MainPlayer(_mainCountry.Id, _mainGame.Id, _mainTeam.Id);
    }

    [Fact]
    public async Task ShouldCreatePlayer()
    {
        // Arrange
        var nickname = "Nickname";
        var rating = 1000;
        var request = new PlayerDto(
            Id: null,
            Nickname: nickname,
            rating: rating,
            Country: null,
            CountryId: _mainCountry.Id.Value,
            Game: null,
            GameId: _mainGame.Id.Value,
            Team: null,
            TeamId: _mainTeam.Id.Value,
            Photo: null,
            UpdateAt: null);

        // Act
        var response = await Client.PostAsJsonAsync("Player", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responsePlayer = await response.ToResponseModel<PlayerDto>();
        var playerId = new PlayerId(responsePlayer.Id!.Value);
        
        var dbPlayer = await Context.Players.FirstAsync(x => x.Id == playerId);
        dbPlayer.NickName.Should().Be(nickname);
        dbPlayer.Rating.Should().Be(rating);
        dbPlayer.CountryId.Value.Should().Be(_mainCountry.Id.Value);
        dbPlayer.GameId.Value.Should().Be(_mainGame.Id.Value);
    }
    [Fact]
    public async Task ShouldNotCreatePlayerBecauseRatingNull()
    {
        // Arrange
        var nickname = "Nickname";
        var rating = 0;
        var request = new PlayerDto(
            Id: null,
            Nickname: nickname,
            rating: rating,
            Country: null,
            CountryId: _mainCountry.Id.Value,
            Game: null,
            GameId: _mainGame.Id.Value,
            Team: null,
            TeamId: _mainTeam.Id.Value,
            Photo: null,
            UpdateAt: null);

        // Act
        var response = await Client.PostAsJsonAsync("Player", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();

     
    }
    public async Task InitializeAsync()
    {
        await Context.Countries.AddAsync(_mainCountry);
        await Context.Games.AddAsync(_mainGame);
        await Context.Players.AddAsync(_mainPlayer);
        
        await Context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Players.RemoveRange(Context.Players);
        Context.Countries.RemoveRange(Context.Countries);
        Context.Games.RemoveRange(Context.Games);
        
        await Context.SaveChangesAsync();
    }
}
