using Domain.Countries;
using Domain.Faculties;
using Domain.Players;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PlayerImageConfiguration : IEntityTypeConfiguration<PlayerImage>
{
    public void Configure(EntityTypeBuilder<PlayerImage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new PlayerImageId(x));
        

        builder.Property(x => x.S3Path)
            .IsRequired()  
            .HasMaxLength(500); 
        
        builder.Property(x => x.PlayerId).HasConversion(x => x.Value, x => new PlayerId(x));
      
        builder.HasOne(x => x.Player)
            .WithMany(g => g.Images)
            .HasForeignKey(x => x.PlayerId)
            .HasConstraintName("fk_game_images_games_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}