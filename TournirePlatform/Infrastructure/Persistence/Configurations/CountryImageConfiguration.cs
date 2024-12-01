using Domain.Countries;
using Domain.Images;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CountryImageConfiguration : IEntityTypeConfiguration<CountryImage>
    {
        public void Configure(EntityTypeBuilder<CountryImage> builder)
        {
            
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new CountryImageId(x));
            
            builder.Property(x => x.S3Path)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.CountryId)
                .HasConversion(x => x.Value, x => new CountryId(x));

            builder.HasOne(x => x.Country) 
                .WithMany()  
                .HasForeignKey(x => x.CountryId)  
                .HasConstraintName("fk_country_images_countries_id")  
                .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}