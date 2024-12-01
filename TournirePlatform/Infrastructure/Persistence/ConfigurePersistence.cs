
using Amazon.S3;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.TournamentFormat;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistence
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuild = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Default"));
        dataSourceBuild.EnableDynamicJson();
        var dataSource = dataSourceBuild.Build();

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseNpgsql(
                    dataSource,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<GameRepository>();
        services.AddScoped<IGameRepositories>(provider => provider.GetRequiredService<GameRepository>());
        services.AddScoped<IGameQueries>(provider => provider.GetRequiredService<GameRepository>());
        
        services.AddScoped<CountryRepository>();
        services.AddScoped<ICountryRepositories>(provider => provider.GetRequiredService<CountryRepository>());
        services.AddScoped<ICountryQueries>(provider => provider.GetRequiredService<CountryRepository>());
        
        services.AddScoped<PlayerRepository>();
        services.AddScoped<IPlayerRepositories>(provider => provider.GetRequiredService<PlayerRepository>());
        services.AddScoped<IPlayerQueries>(provider => provider.GetRequiredService<PlayerRepository>());

        services.AddScoped<TeamRepository>();
        services.AddScoped<ITeamRepository>(provider => provider.GetRequiredService<TeamRepository>());
        services.AddScoped<ITeamQuery>(provider => provider.GetRequiredService<TeamRepository>());
        
        services.AddScoped<MatchRepository>();
        services.AddScoped<IMatchRepository>(provider => provider.GetRequiredService<MatchRepository>());
        services.AddScoped<IMatchQueries>(provider => provider.GetRequiredService<MatchRepository>());

        services.AddScoped<TeamMatchRepository>();
        services.AddScoped<ITeamMatchRepository>(provider => provider.GetRequiredService<TeamMatchRepository>());
        services.AddScoped<ITeamMatchQuery>(provider => provider.GetRequiredService<TeamMatchRepository>());
        
        services.AddScoped<TournamentRepository>();
        services.AddScoped<ITournamentRepository>(provider => provider.GetRequiredService<TournamentRepository>());
        services.AddScoped<ITournamentQueries>(provider => provider.GetRequiredService<TournamentRepository>());
        
        services.AddScoped<FormatRepository>();
        services.AddScoped<IFormatRepository>(provider => provider.GetRequiredService<FormatRepository>());
        services.AddScoped<IFormatQueries>(provider => provider.GetRequiredService<FormatRepository>());

        services.AddScoped<GameImageRepository>();
        services.AddScoped<IGameImageRepository>(provider => provider.GetRequiredService<GameImageRepository>());
        
        /*var mockS3Service = new Mock<IAmazonS3>();
        services.AddSingleton(mockS3Service.Object);*/
        
    }
}