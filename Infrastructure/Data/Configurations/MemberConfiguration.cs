using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastucture.Data.Configurations;

public class MemberConfiguration : BaseEntityConfiguration<Member>
{
    public override void Configure(EntityTypeBuilder<Member> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("Members");

        builder.HasIndex(m => m.EmpNo)
            .IsUnique();

        builder.HasIndex(m => m.Email)
            .IsUnique();

        builder.Property(m => m.EmpNo)            
            .IsRequired()
            .HasMaxLength(EntityConstants.EmpNoLength);

        builder.Property(m => m.FirstName)
            .IsRequired()
            .HasMaxLength(EntityConstants.NameLength);

        builder.Property(m => m.LastName)
            .IsRequired()
            .HasMaxLength(EntityConstants.NameLength);

        builder.Property(m => m.Nominee)
            .HasMaxLength(EntityConstants.NomineeLength);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(EntityConstants.EmailLength);

        builder.Property(m => m.DOJ)
            .IsRequired();

        builder.Property(m => m.Share)
            .HasColumnType("decimal(18,2)");

        // Relationships
        builder.HasOne(m => m.Location)
            .WithMany()
            .HasForeignKey(m => m.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
