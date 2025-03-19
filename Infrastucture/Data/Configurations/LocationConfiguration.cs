using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastucture.Data.Configurations;

public class LocationConfiguration : BaseEntityConfiguration<Location>
{
    public override void Configure(EntityTypeBuilder<Location> builder)
    {
        base.Configure(builder);
        // Table Name
        builder.ToTable("Locations");
        // Properties
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(EntityConstants.NameLength);        
    }
}
