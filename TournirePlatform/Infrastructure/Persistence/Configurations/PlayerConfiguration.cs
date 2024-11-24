using Domain.Players;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new PlayerId(x));
        
        builder.Property(x => x.NickName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Rating).IsRequired();
        
        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .HasConstraintName("FK_Player_Country")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Game)
            .WithMany()
            .HasForeignKey(x => x.GameId)
            .HasConstraintName("FK_Player_Game")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Team)
            .WithMany()
            .HasForeignKey(x => x.TeamId)
            .HasConstraintName("FK_Player_Team")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Photo)
            .IsRequired(false)
            .HasColumnType("bytea");

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        
        
    }
}