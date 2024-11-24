using System.Security.Cryptography.X509Certificates;
using Domain.Matches;
using Domain.TeamsMatchs;
using Domain.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TeamMatchConfiguration : IEntityTypeConfiguration<TeamMatch>
{
    public void Configure(EntityTypeBuilder<TeamMatch> builder)
    {
        builder. HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new TeamMatchId(x)).IsRequired();   
        builder.Property(x => x.TeamId).HasConversion(x => x.Value, x => new TeamId(x)).IsRequired();
        builder.Property(x => x.MatchId).HasConversion(x => x.Value, x => new MatchId(x)).IsRequired();
        builder.Property(x => x.Score).IsRequired();

        builder.HasOne(x => x.Team)
            .WithMany(t => t.TeamMatches)
            .HasForeignKey(x => x.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.MatchGame)
            .WithMany(x => x.TeamMatches)
            .HasForeignKey(x => x.MatchId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}