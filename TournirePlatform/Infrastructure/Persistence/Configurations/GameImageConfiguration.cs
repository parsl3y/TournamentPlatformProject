using Domain.Countries;
using Domain.Faculties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class GameImageConfiguration : IEntityTypeConfiguration<GameImage>
{
    public void Configure(EntityTypeBuilder<GameImage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new GameImageId(x));
        

        builder.Property(x => x.S3Path)
            .IsRequired()  
            .HasMaxLength(500); 
        
        builder.Property(x => x.GameId).HasConversion(x => x.Value, x => new GameId(x));
      
        builder.HasOne(x => x.Game)
            .WithMany(g => g.Images)
            .HasForeignKey(x => x.GameId)
            .HasConstraintName("fk_game_images_games_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}