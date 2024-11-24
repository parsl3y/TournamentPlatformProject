using Domain.Players;
using Domain.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new TeamId(x));
        
        builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(225)");
        builder.Property(x => x.Icon).IsRequired(false);
        builder.Property(x => x.MatchCount).IsRequired();
        builder.Property(x => x.WinCount).IsRequired();
        builder.Property(x => x.WinRate).IsRequired();
        builder.Property(x => x.CreationDate).IsRequired();
        
        builder.HasMany(x => x.PlayerTeams)
            .WithOne(x => x.Team)
            .HasForeignKey(x => x.TeamId);
        
        builder.HasMany(x => x.TeamMatches)
            .WithOne()
            .HasForeignKey(x => x.MatchId);
    }
}