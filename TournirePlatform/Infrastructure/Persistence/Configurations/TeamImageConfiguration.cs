using Domain.Countries;
using Domain.Faculties;
using Domain.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TeamImageConfiguration: IEntityTypeConfiguration<TeamImage>
{
    public void Configure(EntityTypeBuilder<TeamImage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new TeamImageId(x));
        

        builder.Property(x => x.S3Path)
            .IsRequired()  
            .HasMaxLength(500); 
        
        builder.Property(x => x.TeamId).HasConversion(x => x.Value, x => new TeamId(x));
      
        builder.HasOne(x => x.Team)
            .WithMany(g => g.Images)
            .HasForeignKey(x => x.TeamId)
            .HasConstraintName("fk_game_images_games_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}