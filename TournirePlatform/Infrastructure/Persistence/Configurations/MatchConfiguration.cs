using System.Collections.Immutable;
using Domain.Matches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<MatchGame>
{
    public void Configure(EntityTypeBuilder<MatchGame> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new MatchId(x));
        
        builder.HasOne(x => x.Game)
            .WithMany()
            .HasForeignKey(x => x.GameId)
            .HasConstraintName("FK_Player_Game")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Tournament)
            .WithMany()
            .HasForeignKey(x => x.TournamentId)
            .HasConstraintName("FK_Player_Tournament")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.StartAt).IsRequired();
        builder.Property(x => x.MaxTeams).IsRequired();

        builder.HasMany(x => x.TeamMatches)
            .WithOne()
            .HasForeignKey(x => x.MatchId);
    }
}