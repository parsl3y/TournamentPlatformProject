using Domain.Tournaments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.Configurations;

public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
{
    public void Configure(EntityTypeBuilder<Tournament> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x=> new TournamentId(x));
        
        builder.Property(x => x.Name).HasMaxLength(225).IsRequired();
        builder.Property(x => x.StartDate).HasColumnType("date").IsRequired();
        
        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .HasConstraintName("FK_Tournament_Country")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Game)
            .WithMany()
            .HasForeignKey(x => x.GameId)
            .HasConstraintName("FK_Tournament_Game")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FormatTournament)
            .WithMany()
            .HasForeignKey(x => x.FormatTournamentId)
            .HasConstraintName("FK_Tournament_Format")
            .OnDelete(DeleteBehavior.Restrict);

        
        builder.HasMany(x => x.matchGames)
            .WithOne(x => x.Tournament)
            .HasForeignKey(x => x.TournamentId);

    }
}