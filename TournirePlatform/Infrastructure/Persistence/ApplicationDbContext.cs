using System.Reflection;
using Domain.Countries;
using Domain.Faculties;
using Domain.Game;
using Domain.Matches;
using Domain.Players;

using Domain.TeamsMatchs;
using Domain.Teams;
using Domain.TournamentFormat;
using Domain.Tournaments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<MatchGame> Matches { get; set; }
    public DbSet<TeamMatch> TeamMatches { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Format> Formats { get; set; }
    public DbSet<GameImage> GameImages { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}